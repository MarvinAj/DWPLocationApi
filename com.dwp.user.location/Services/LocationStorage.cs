
using com.dwp.user.location.Exceptions;
using com.dwp.user.location.Model;

namespace com.dwp.user.location.Services
{
    public class LocationStorage : ILocationStorage
    {
        private List<City> _cityList;

        public LocationStorage()
        {
            _cityList = CreateCities();
        }

        public City GetCity(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("City name is required");

            var city = _cityList.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));

            if (city == null) throw new CityNotFoundException(name);            

            return city;
        }


        public IEnumerable<City> GetCities()
        {
            return _cityList;
        }

        private List<City> CreateCities()
        {
            return new List<City>()
            {
                new City(){
                    Name = "London",
                    Coordinate = new Coordinate(51.507222,-0.1275)
                },
                new City(){
                    Name = "Manchester",
                    Coordinate = new Coordinate(53.483959,-2.244644)
                },
                new City(){
                    Name = "Cambridge",
                    Coordinate = new Coordinate(52.205276,0.119167)
                },
                new City(){
                    Name = "Oxford",
                    Coordinate = new Coordinate(51.752022,-1.257677)
                },
                new City(){
                    Name = "Bristol",
                    Coordinate = new Coordinate(51.454514,-2.587910)
                }
            };
        }
    }
}
