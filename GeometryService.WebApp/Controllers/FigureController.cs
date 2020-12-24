using System;
using System.Threading.Tasks;
using GeometryService.Domain.Models;
using GeometryService.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace GeometryService.WebApp.Controllers
{
    /// <summary>
    /// Controller for figure executing
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class FigureController : ControllerBase
    {
        private readonly ILogger<FigureController> _logger;
        private readonly IFigureService _figureService;

        public FigureController(ILogger<FigureController> logger, IFigureService figureService)
        {
            _figureService = figureService;
            _logger = logger;
        }

        /// <summary>
        /// Figure creation
        /// </summary>
        /// <param name="figure"></param>
        /// <returns>Figure id in database</returns>
        /// <response code="200"> Successful operation. </response>
        /// <response code="400"> Something was wrong by input data. </response>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> CreateFigureAsync([FromBody] JObject figure)
        {
            if (figure == null)
            {
                return BadRequest("figure == null");
            }

            var type = figure[nameof(FigureBase.Type)].ToObject<FigureType>();
            FigureBase entity = type switch
            {
                FigureType.Circle => figure.ToObject<CircleFigure>(),
                FigureType.Triangle => figure.ToObject<TriangleFigure>(),
                _ => throw new ArgumentOutOfRangeException(type.ToString())
            };
            
            if (!(entity?.IsValid() ?? false))
            {
                return BadRequest("Can't convert figure");
            }

            try
            {
                var id = await _figureService.CreateFigureAsync(entity);
                return Ok(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Bad creation figure request");
                return BadRequest("Bad creation figure request");
            }
        }

        /// <summary>
        /// Get figure square
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Figure square</returns>
        /// <response code="200"> Successful operation. </response>
        /// <response code="400"> Something was wrong by input data. </response>
        /// <response code="404"> Figure is not found. </response>
        /// <response code="500"> Bad data on server. </response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<double>> GetFigureSquareAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Bad param Id");
            }

            if (!Guid.TryParse(id, out var figureId))
            {
                return BadRequest("Can't parse Id");
            }

            try
            {
                var figure = await _figureService.GetFigureAsync(figureId);
                if (figure == null)
                {
                    return NotFound();
                }

                if (!figure.IsValid())
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Figure is not valid");
                }
                
                return Ok(figure.Square());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Bad figure square request");
                return BadRequest("BBad figure square request");
            }
        }
    }
}
