

using com.dwp.user.location.Exceptions;
using com.dwp.user.location.Services;
using System;
using Xunit;

namespace com.dwp.user.location.tests
{
    public class LocationStorageTests
    {
        private ILocationStorage locationStorage;
        public LocationStorageTests()
        {
            locationStorage = new LocationStorage();
        }

        [Fact]
        public void LocationStorage_GetCityWithEmtyOrNullString_ThrowsArgumentException()
        {
            var city = string.Empty;
            string nullCity = null;
            Assert.Throws<ArgumentException>(() => locationStorage.GetCity(city));
            Assert.Throws<ArgumentException>(() => locationStorage.GetCity(nullCity));
        }


        [Fact]
        public void LocationStorage_GetCityWithUnknownCity_ThrowsCityNotFoundException()
        {
            var city = "UnKnownCity";
            Assert.Throws<CityNotFoundException>(() => locationStorage.GetCity(city));
        }

        [Fact]
        public void LocationStorage_GetCity_ReturnsExpectedCity()
        {
            var name = "London";
            var latidude = 51.507222;
            var longitute = -0.1275;

            var city = locationStorage.GetCity(name);

            Assert.Equal(name, city.Name);
            Assert.Equal(longitute, city.Coordinate.Longitude);
            Assert.Equal(latidude, city.Coordinate.Latitude);
        }

        [Fact]
        public void LocationStorage_GetCities_ReturnsCollectionOfCities()
        {
            var cities = locationStorage.GetCities();
            Assert.NotEmpty(cities);
        }

    }
}
