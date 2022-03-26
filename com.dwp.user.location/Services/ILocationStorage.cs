
using com.dwp.user.location.Model;

namespace com.dwp.user.location.Services
{
    public interface ILocationStorage
    {
        public City GetCity(string name);
        public IEnumerable<City> GetCities();

    }
}
