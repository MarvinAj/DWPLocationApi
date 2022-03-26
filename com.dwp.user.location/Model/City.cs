
using System.ComponentModel.DataAnnotations;

namespace com.dwp.user.location.Model
{
    public class City
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public Coordinate Coordinate { get; set; }
    }
}
