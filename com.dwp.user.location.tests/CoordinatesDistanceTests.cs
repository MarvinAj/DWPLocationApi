
using com.dwp.user.location.Model;
using System;
using Xunit;

namespace com.dwp.user.location.tests
{
    public class CoordinatesDistanceTests
    {
        [Fact]
        public void Coordinate_SetLatitude_ThrowsOnInvalidValues()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Coordinate(90.1, double.NaN));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Coordinate(-90.1, double.NaN));
        }

        [Fact]
        public void Coordinate_SetLongitude_ThrowsOnInvalidValues()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Coordinate(double.NaN, 180.1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Coordinate(double.NaN, -180.1));
        }

        [Fact]
        public void Coordinate_EqualsOperatorWithNullParameters_DoesNotThrow()
        {
            Coordinate? first = null;
            Coordinate? second = null;
            Assert.True(first == second);

            first = new Coordinate(double.NaN, double.NaN);
            Assert.False(first == second);

            first = null;
            second = new Coordinate(double.NaN, double.NaN);
            Assert.False(first == second);
        }

        [Fact]
        public void CoordinatesDistance_DistanceToWithNaNCoordinates_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Coordinate(double.NaN, 1).DistanceTo(new Coordinate(5, 5)));
            Assert.Throws<ArgumentException>(() => new Coordinate(1, double.NaN).DistanceTo(new Coordinate(5, 5)));
            Assert.Throws<ArgumentException>(() => new Coordinate(1, 1).DistanceTo(new Coordinate(double.NaN, 5)));
            Assert.Throws<ArgumentException>(() => new Coordinate(1, 1).DistanceTo(new Coordinate(5, double.NaN)));
        }


        [Fact]
        public void CoordinatesDistance_DistanceToWithNullCoordinates_ThrowsArgumentNullException()
        {
            Coordinate coordinate1 = null;
            Coordinate coordinate2 = null;
            Coordinate coordinate = new Coordinate(5, 5);
            Assert.Throws<ArgumentNullException>(() => coordinate1.DistanceTo(coordinate));
            Assert.Throws<ArgumentNullException>(() => coordinate.DistanceTo(coordinate2));
            Assert.Throws<ArgumentNullException>(() => coordinate1.DistanceTo(coordinate2));
        }

        [Fact]
        public void CoordinatesDistance_DistanceToWithEqualCoordinates_ReturnsZero()
        {
            var start = new Coordinate(5, 5);
            var end = new Coordinate(5, 5);
            var distance = start.DistanceTo(end);
            var expected = 0;

            Assert.Equal(expected, distance);
        }

        [Fact]
        public void CoordinatesDistance_DistanceTo_ReturnsExpectedDistance()
        {
            var start = new Coordinate(1, 1);
            var end = new Coordinate(5, 5);
            var distance = start.DistanceTo(end);
            var expected = 629060.759879635;
            var delta = distance - expected;

            Assert.True(delta < 1e-8);
        }

    }
}
