using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.dwp.user.location.Exceptions
{
    public class CityNotFoundException : Exception
    {
        public CityNotFoundException(string city)
           : base($"Could not find city with name {city}")
        { }
    }
}
