using System;
using System.ComponentModel.DataAnnotations;

namespace GeometryService.Domain.Models
{
    /// <summary>
    /// Circle figure implementation
    /// </summary>
    public class CircleFigure : FigureBase
    {
        /// <inheritdoc />
        public override FigureType Type => FigureType.Circle;

        /// <summary>
        /// Coordinates of circle's center
        /// </summary>
        public Point2D Center { get; set; }
        
        /// <summary>
        /// Circle's radius
        /// </summary>
        public double Radius { get; set; }

        /// <inheritdoc />
        public override double Square()
        {
            return Math.PI * Radius * Radius;
        }

        public override bool IsValid()
        {
            return Center != null;
        }
    }
}