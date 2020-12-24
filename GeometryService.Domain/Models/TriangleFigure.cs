using System;
using System.ComponentModel.DataAnnotations;

namespace GeometryService.Domain.Models
{
    /// <summary>
    /// Triangle figure implementation
    /// </summary>
    public class TriangleFigure : FigureBase
    {
        /// <inheritdoc />
        public override FigureType Type => FigureType.Triangle;

        /// <summary>
        /// Coordinates of first point
        /// </summary>
        [Required]
        public Point2D Point1 { get; set; }

        /// <summary>
        /// Coordinates of second point
        /// </summary>
        [Required]
        public Point2D Point2 { get; set; }

        /// <summary>
        /// Coordinates of third point
        /// </summary>
        [Required]
        public Point2D Point3 { get; set; }

        /// <inheritdoc />
        public override double Square()
        {
            if (!IsValid())
            {
                return 0;
            }
            
            // triangle's square is found by Heron's formula

            var a = Point2D.Distance(Point1, Point2);
            var b = Point2D.Distance(Point2, Point3);
            var c = Point2D.Distance(Point3, Point1);
            var p = 0.5 * (a + b + c);

            return Math.Sqrt(p* (p - a) * (p - b) * (p - c));
        }

        public override bool IsValid()
        {
            return Point1 != null && Point2 != null && Point3 != null;
        }
    }
}