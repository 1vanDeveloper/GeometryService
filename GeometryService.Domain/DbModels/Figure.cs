using GeometryService.Domain.Models;

namespace GeometryService.Domain.DbModels
{
    /// <summary>
    /// Database figure entity
    /// </summary>
    public class Figure: BaseEntity
    {
        /// <summary>
        /// Type of implemented figure
        /// </summary>
        public FigureType Type { get; set; }
        
        /// <summary>
        /// Serialized figure
        /// </summary>
        public string Data { get; set; }
    }
}