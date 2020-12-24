using System;
using System.Threading.Tasks;
using GeometryService.Domain.DbModels;
using GeometryService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GeometryService.Domain.Services
{
    /// <inheritdoc cref="IFigureService"/>
    public class FigureService : IFigureService
    {
        private readonly ILogger<IFigureService> _logger;
        private readonly AppDbContext _dbContext;

        public FigureService(ILogger<IFigureService> logger, AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task<Guid> CreateFigureAsync(FigureBase figure)
        {
            try
            {
                figure.Id = Guid.Empty;
                
                var figureEntity = new Figure
                {
                    Type = figure.Type,
                    Data = JsonConvert.SerializeObject(figure)
                };

                var newRow = await _dbContext.Figures.AddAsync(figureEntity);
                await _dbContext.SaveChangesAsync();
                return newRow.Entity.Id;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Save new entity exception");
                throw;
            }
        }

        public async Task<FigureBase> GetFigureAsync(Guid id)
        {
            try
            {
                var figureEntity = await _dbContext.Figures.FirstOrDefaultAsync(f => f.Id == id);
                if (figureEntity == null)
                {
                    return null;
                }
                
                FigureBase figure = figureEntity.Type switch
                {
                    FigureType.Circle => JsonConvert.DeserializeObject<CircleFigure>(figureEntity.Data),
                    FigureType.Triangle => JsonConvert.DeserializeObject<TriangleFigure>(figureEntity.Data),
                    _ => throw new ArgumentOutOfRangeException(figureEntity.Type.ToString())
                };

                figure.Id = id;

                return figure;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Get entity exception");
                throw;
            }
        }
    }
}