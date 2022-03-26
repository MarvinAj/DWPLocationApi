
using System.ComponentModel.DataAnnotations;

namespace com.dwp.user.location.Model
{
    public class Coordinate
    {    
        public Coordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        private double latitude;
        [Required]
        public double Latitude 
        {
            get { return latitude; } 
            private set 
            {
                if (value > 90.0 || value < -90.0)
                {
                    throw new ArgumentOutOfRangeException("Latitude", "Argument must be in range of -90 to 90");
                }
                latitude = value; 
            } 
        }

        private double longitude;
        [Required]
        public double Longitude 
        {
            get { return longitude; } 
            private set
            {
                if (value > 180.0 || value < -180.0)
                {
                    throw new ArgumentOutOfRangeException("Longitude", "Argument must be in range of -180 to 180");
                }
                longitude = value;
            } 
        }

    }
}
