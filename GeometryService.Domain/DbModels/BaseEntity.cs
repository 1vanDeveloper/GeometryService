using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeometryService.Domain.DbModels
{
    /// <summary>
    /// Base class for all entities
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
    }
}