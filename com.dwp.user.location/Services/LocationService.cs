using com.dwp.user.location.Exceptions;
using com.dwp.user.location.Extensions;
using com.dwp.user.location.Model;
using Newtonsoft.Json;

namespace com.dwp.user.location.Services
{
    public class LocationService: ILocationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocationStorage _locationStorage;

        public LocationService(IHttpClientFactory httpClientFactory, ILocationStorage locationStorage)
        {
            _httpClientFactory = httpClientFactory.ThrowIfNull(nameof(httpClientFactory));
            _locationStorage = locationStorage.ThrowIfNull(nameof(locationStorage));
        }

        /// <summary>
        /// Retrieves all users 
        /// </summary>
        /// <returns>
        /// A list of users
        /// </returns>
        /// <exception cref="UserLocationException"></exception>
        /// <exception cref="UsersNotFoundException"></exception>
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, Environment.GetEnvironmentVariable("UsersURL")));

            if (!response.IsSuccessStatusCode)
            {
                throw new UserLocationException("Failed to retrieve users", response);
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<User>>(content);

            if(result == null)
            {
                throw new UsersNotFoundException("Could not find any users");
            }

            return result;
        }

        /// <summary>
        /// Retrieves a list of users within radius of a given coordinate
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="coordinate"></param>
        /// <returns>
        /// List of users within radius of a given coordinate
        /// </returns>
        /// <exception cref="UserLocationException"></exception>
        /// <exception cref="UsersNotFoundException"></exception>
        public async Task<IEnumerable<User>> GetUsersAsync(double radius, Coordinate coordinate)
        {
            if(double.IsNaN(radius)) throw new ArgumentException($"Argument {nameof(radius)} is not a number");
            if(coordinate == null) throw new ArgumentNullException(nameof(coordinate));
            if (double.IsNaN(coordinate.Latitude) || double.IsNaN(coordinate.Longitude)) 
            {
                throw new ArgumentException($"Argument {nameof(coordinate)} latitude or longitude is not a number");
            }

            var users = await GetUsersAsync();
            return users.Where(x =>  coordinate.DistanceTo(new Coordinate(x.Latitude, x.Longitude)) <= radius);
        }

        /// <summary>
        /// Retrieves a list of users within a given radius of a city
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="cityName"></param>
        /// <returns>
        /// List of users within a given radius of a city
        /// </returns>
        /// <exception cref="UserLocationException"></exception>
        /// <exception cref="UsersNotFoundException"></exception>
        public async Task<IEnumerable<User>> GetUsersAsync(double radius,string cityName)
        {
            var city = await GetCityAsync(cityName);
            return await GetUsersAsync(radius, city.Coordinate);
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            await Task.CompletedTask;
            return _locationStorage.GetCities();
        }

        public async Task<City> GetCityAsync(string cityName)
        {
            await Task.CompletedTask;
            return _locationStorage.GetCity(cityName);
        }

    }
}
