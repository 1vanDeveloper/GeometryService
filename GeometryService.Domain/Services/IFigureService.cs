using System;
using System.Threading.Tasks;
using GeometryService.Domain.Models;

namespace GeometryService.Domain.Services
{
    /// <summary>
    /// CRUD for figures
    /// </summary>
    public interface IFigureService
    {
        /// <summary>
        /// Create figure at database
        /// </summary>
        /// <param name="figure"></param>
        /// <returns></returns>
        Task<Guid> CreateFigureAsync(FigureBase figure);

        /// <summary>
        /// Get figure from database by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<FigureBase> GetFigureAsync(Guid id);
    }
}