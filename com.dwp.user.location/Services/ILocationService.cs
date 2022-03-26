

using com.dwp.user.location.Model;

namespace com.dwp.user.location.Services
{
    public interface ILocationService
    {
        public Task<IEnumerable<User>> GetUsersAsync();
        public Task<IEnumerable<User>> GetUsersAsync(double radius, Coordinate coordinates);
        public Task<IEnumerable<User>> GetUsersAsync(double radius, string cityName);
        public Task<IEnumerable<City>> GetCitiesAsync();
        public Task<City> GetCityAsync(string cityName);

    }
}
