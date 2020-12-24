using System;

namespace GeometryService.Domain.Models
{
    /// <summary>
    /// The base class of figure
    /// </summary>
    public abstract class FigureBase
    {
        /// <summary>
        /// Figure id from db
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Type of implemented figure
        /// </summary>
        public abstract FigureType Type { get; }

        /// <summary>
        /// Square of figure in units of Point
        /// </summary>
        /// <returns></returns>
        public abstract double Square();

        /// <summary>
        /// Check figure on valid parameters
        /// </summary>
        /// <returns></returns>
        public abstract bool IsValid();
    }
}