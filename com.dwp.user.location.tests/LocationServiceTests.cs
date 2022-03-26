using com.dwp.user.location.Exceptions;
using com.dwp.user.location.Model;
using com.dwp.user.location.Services;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace com.dwp.user.location.tests
{
    public class LocationServiceTests
    {
        private readonly IEnumerable<User> _users;
        private readonly Coordinate _coordinateLondon;
        private readonly Mock<ILocationStorage> _locationStorageMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

        public LocationServiceTests()
        {
            _users = new List<User>{
                new User{Id = 1, Email = "mbox@test.com",FirstName ="Ancell", LastName ="Garnsworthy", Latitude = 51.6553959,Longitude= 0.0572553 },
                new User{Id = 2, Email = "mb1@test.com",FirstName ="John", LastName ="Snow", Latitude = 51.6710832,Longitude= 0.8078532 },
                new User{Id = 2, Email = "mb2@test.com",FirstName ="Jack", LastName ="Smith", Latitude = 51.5489435,Longitude= 0.3860497 },
                new User{Id = 2, Email = "mb3@test.com",FirstName ="Mark", LastName ="Dickson", Latitude = 2,Longitude= 3 },
            };

            _coordinateLondon = new Coordinate(51.507222, -0.1275);
            _locationStorageMock = new Mock<ILocationStorage>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        }

        [Fact]
        public void LocationService_GetUsersAsyncWithNotSuccessStatusCode_ThrowsUserLocationException()
        {
            //Arrange
            var argExMessage = "Failed to retrieve users";

            //Act
            var locationService = SetupLocationService(HttpStatusCode.BadRequest, string.Empty);

            //Assert
            var ex = Assert.ThrowsAsync<UserLocationException>(Task() => locationService.GetUsersAsync());
            Assert.Equal(argExMessage, ex.Result.Message);
        }

        [Fact]
        public void LocationService_GetUsersAsyncWithIsSuccessStatusCodeAndNoContent_ThrowsUsersNotFoundException()
        {
            //Arrange
            var argExMessage = "Could not find any users";

            //Act
            var locationService = SetupLocationService(HttpStatusCode.OK, string.Empty);

            //Assert
            var ex = Assert.ThrowsAsync<UsersNotFoundException>(Task () => locationService.GetUsersAsync());
            Assert.Equal(argExMessage, ex.Result.Message);
        }

        [Fact]
        public void LocationService_GetUsersAsync_ReturnExpectedUsers()
        {
            //Arrange
            var users = _users;
            var content = JsonConvert.SerializeObject(users);
            var locationService = SetupLocationService(HttpStatusCode.OK, content);

            //Act
            var result = locationService.GetUsersAsync().Result;

            //Assert
            Assert.NotEmpty(result);
            Assert.Equal(users.First().Email, result.First().Email);
        }

        [Fact]
        public void LocationService_GetUsersAsyncWithEmtyNaNRadius_ThrowsArgumentException()
        {
            //Arrange
            var locationService = SetupLocationService(HttpStatusCode.OK, string.Empty);
            var argExMessage = "Argument radius is not a number";

            //Act
            //Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(Task() => locationService.GetUsersAsync(double.NaN,new Coordinate(1,1)));
            Assert.Equal(argExMessage, ex.Result.Message);
        }

        [Fact]
        public void LocationService_GetUsersAsyncWithEmtyNullCoordinate_ThrowsArgumentNullException()
        {
            //Arrange
            var locationService = SetupLocationService(HttpStatusCode.OK, string.Empty);
            var argExMessage = "Value cannot be null. (Parameter 'coordinate')";
            Coordinate coordinate = null;

            //Act
            Task act() => locationService.GetUsersAsync(30, coordinate);

            //Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(act);
            Assert.Equal(argExMessage, ex.Result.Message);
        }

        [Fact]
        public void LocationService_GetUsersAsyncWithNaNCoordinate_ThrowsArgumentException()
        {
            //Arrange
            var locationService = SetupLocationService(HttpStatusCode.OK, string.Empty);
            var argExMessage = "Argument coordinate latitude or longitude is not a number";
            var radius = 30;

            //Act
            //Assert
            Assert.ThrowsAsync<ArgumentException>(Task () => locationService.GetUsersAsync(radius, new Coordinate(double.NaN, 1)));
            Assert.ThrowsAsync<ArgumentException>(Task () => locationService.GetUsersAsync(radius, new Coordinate(1, double.NaN)));
            var ex = Assert.ThrowsAsync<ArgumentException>(Task () => locationService.GetUsersAsync(radius, new Coordinate(double.NaN, double.NaN)));
            Assert.Equal(argExMessage, ex.Result.Message);
        }

        [Fact]
        public void LocationService_GetUsersAsyncUsersWithin50MilesRadiusOfCoordinate_ReturnUsersWithinRadius()
        {
            //Arrange
            var radius = 50;
            var content = JsonConvert.SerializeObject(_users);
            var locationService = SetupLocationService(HttpStatusCode.OK, content);
            var expectedCount = 3;

            //Act
            var users = locationService.GetUsersAsync(radius, _coordinateLondon);

            //Assert
            Assert.NotEmpty(users.Result);
            Assert.True(users.Result.Count() == expectedCount);
        }

        [Fact]
        public void LocationService_GetUsersAsyncUsersWithin50MilesRadiusOfLondon_ReturnUsers()
        {
            //Arrange
            var radius = 50;
            var content = JsonConvert.SerializeObject(_users);
            var locationService = SetupLocationService(HttpStatusCode.OK, content);
            var expectedCount = 3;

            _locationStorageMock.Setup(x => x.GetCity(It.IsAny<string>())).Returns(new City()
            {
                Name = "London",
                Coordinate = _coordinateLondon
            });
            
            //Act
            var users = locationService.GetUsersAsync(radius, "London");

            //Assert
            Assert.NotEmpty(users.Result);
            Assert.True(users.Result.Count() == expectedCount);
            _locationStorageMock.Verify(x => x.GetCity(It.IsAny<string>()),Times.Once);
            _httpClientFactoryMock.Verify(x => x.CreateClient(It.IsAny<string>()), Times.Once);
        }

        private ILocationService SetupLocationService(HttpStatusCode statusCode, string content)
        {
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(content)
                });

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            httpClient.BaseAddress = new Uri("https://test.com/api/test");

            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            return new LocationService(_httpClientFactoryMock.Object, _locationStorageMock.Object);
        }
    }
}