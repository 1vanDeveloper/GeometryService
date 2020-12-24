using System;

namespace GeometryService.Domain.Models
{
    /// <summary>
    /// Point's coordinates in 2D measurement
    /// </summary>
    public class Point2D
    {
        /// <summary>
        /// Coordinate of X-axis
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Coordinate of Y-axis
        /// </summary>
        public double Y { get; set; }

        public override string ToString()
        {
            return $"({X}; {Y})";
        }

        /// <summary>
        /// Distance between two points
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static double Distance(Point2D point1, Point2D point2)
        {
            var xDelta = point1.X - point2.X;
            var yDelta = point1.Y - point2.Y;

            return Math.Sqrt(xDelta * xDelta + yDelta * yDelta);
        }
    }
}