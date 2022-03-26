using com.dwp.user.location.Model;

namespace com.dwp.user.location
{
    public static class CoordinatesDistance
    {
        private const double _hundredAndEightyDegrees = 180.0;

        /// <summary>
        /// Distance calculation using the Spherical Law of Cosines
        /// </summary>
        /// <param name="baseCoordinate"></param>
        /// <param name="targetCoordinate"></param>
        /// <returns>
        /// Distance between two coordinates in miles
        /// </returns>
        public static double DistanceTo(this Coordinate baseCoordinate, Coordinate targetCoordinate)
        {
            if (baseCoordinate == null) throw new ArgumentNullException(nameof(baseCoordinate));
            if (targetCoordinate == null) throw new ArgumentNullException(nameof(targetCoordinate));

            if (double.IsNaN(baseCoordinate.Latitude) || 
                double.IsNaN(baseCoordinate.Longitude) || 
                double.IsNaN(targetCoordinate.Latitude) ||
                double.IsNaN(targetCoordinate.Longitude))
            {
                throw new ArgumentException("Argument latitude or longitude is not a number");
            }

            if ((baseCoordinate.Latitude == targetCoordinate.Latitude) && (baseCoordinate.Longitude == targetCoordinate.Longitude))
            {
                return 0;
            }

            var baseRad = baseCoordinate.Latitude.ConvertDegreesToRadian();
            var targetRad = targetCoordinate.Latitude.ConvertDegreesToRadian();

            var theta = baseCoordinate.Longitude - targetCoordinate.Longitude;
            var thetaRad = theta.ConvertDegreesToRadian();

            double distance = Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) * Math.Cos(targetRad) * Math.Cos(thetaRad);

            distance = Math.Acos(distance);
            distance = distance.ConvertRadianToDegrees();
            distance = distance.ConvertDegreesToMiles();

            return distance;
        }

        private static double ConvertDegreesToMiles(this double degrees)
        {
            //1 Nautical mile = 1.15078 Statute miles or Land Mile
            var nauticalToStatuteMiles = 1.15078;

            // 1 degree = 60 nautical miles
            var degreeToNauticalMiles = 60;

            return degrees * degreeToNauticalMiles * nauticalToStatuteMiles;
        }

        private static double ConvertDegreesToRadian(this double degrees)
        {
            return Math.PI * degrees / _hundredAndEightyDegrees;
        }

        private static double ConvertRadianToDegrees(this double radian)
        {
            return radian * (_hundredAndEightyDegrees / Math.PI);
        }

    }

}
