
using Newtonsoft.Json;

namespace com.dwp.user.location.Model
{
    public class User
    {
        public int Id { get; set; }
        
        [JsonProperty("first_name")]
        public string FirstName { get; set; } = String.Empty;
        
        [JsonProperty("last_name")]
        public string LastName { get; set; } = String.Empty;
        
        public string Email { get; set; } = String.Empty;
        
        [JsonProperty("ip_address")]
        public string IpAddress { get; set; } = String.Empty;
        
        public double Latitude { get; set; }
        
        public double Longitude { get; set; }
    }
}
