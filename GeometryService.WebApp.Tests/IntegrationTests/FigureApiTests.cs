using System;
using System.Threading.Tasks;
using FluentAssertions;
using GeometryService.Domain.Models;
using GeometryService.Domain.Services;
using GeometryService.WebApp.Controllers;
using GeometryService.WebApp.Tests.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GeometryService.WebApp.Tests.IntegrationTests
{
    /// <summary>
    /// Tests for <see cref="FigureController"/>
    /// </summary>
    public class FigureApiTests : IntegrationTestsBase
    {
        [Test]
        public async Task CreateNullFigureTypeTestAsync()
        {
            // ACT
            var setResult = await PostAsync($"api/figure", GetHttpContentJsonBody(new object()));
            setResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest, "FigureBase.Type == null");
        }

        [Test]
        public async Task CreateBadFigureTestAsync()
        {
            var body1 = new
            {
                Type = 10
            };
            
            // ACT
            var setResult = await PostAsync($"api/figure", GetHttpContentJsonBody(body1));
            setResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest, "Can't convert figure");

            var body2 = new
            {
                Type = 0,
                Center = (Point2D)null,
                Radius = 1
            };

            // ACT
            setResult = await PostAsync($"api/figure", GetHttpContentJsonBody(body2));
            setResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest, "Can't convert figure");

            var body3 = new
            {
                Type = 1,
                Point1 = (Point2D)null,
                Point2 = new Point2D
                {
                    X = 2,
                    Y = 2
                },
                Point3 = new Point2D
                {
                    X = 1,
                    Y = 1
                }
            };

            // ACT
            setResult = await PostAsync($"api/figure", GetHttpContentJsonBody(body3));
            setResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest, "Can't convert figure");
        }

        [Test]
        public async Task GetSquareBadParamTestAsync()
        {
            // ACT
            var getResult = await GetAsync($"api/figure/1.1.1.1");
            getResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest, "Can't parse Id");
        }

        [Test]
        public async Task GetSquareNonExistParamTestAsync()
        {
            // ACT
            var getResult = await GetAsync($"api/figure/{Guid.NewGuid()}");
            getResult.StatusCode.Should().Be(StatusCodes.Status404NotFound, "Not found");
        }

        [Test]
        public async Task GetInvalidSquareParamTestAsync()
        {
            using var scope = Factory.Services.CreateScope();
            var figureService = scope.ServiceProvider.GetService<IFigureService>();

            var invalidFigure = new CircleFigure
            {
                Center = null,
                Radius = 1
            };

            var newId = await figureService.CreateFigureAsync(invalidFigure);
            
            // ACT
            var getResult = await GetAsync($"api/figure/{newId}");
            getResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError, "Invalid figure");
        }

        [Test]
        public async Task CreateTriangleAndGetSquareFigureTestAsync()
        {
            var body = new
            {
                Type = 1,
                Point1 = new Point2D
                {
                    X = 1,
                    Y = 1
                },
                Point2 = new Point2D
                {
                    X = 2,
                    Y = 1
                },
                Point3 = new Point2D
                {
                    X = 2,
                    Y = 2
                }
            };

            // ACT
            var setResult = await PostAsync($"api/figure", GetHttpContentJsonBody(body));
            setResult.StatusCode.Should().Be(StatusCodes.Status200OK, "Triangle is created");
            
            var answer = await setResult.Content.ReadAsStringAsync();
            var triangleId = JsonConvert.DeserializeObject<Guid>(answer);
            triangleId.Should().NotBe(Guid.Empty);

            var getResult = await GetAsync($"api/figure/{triangleId}");
            getResult.StatusCode.Should().Be(StatusCodes.Status200OK, "Valid figure square");
            var result = await getResult.Content.ReadAsStringAsync();

            var square = JsonConvert.DeserializeObject<double>(result);
            Math.Abs(square - 0.5).Should().BeLessThan(0.000001);
        }
        
        [Test]
        public async Task CreateCircleAndGetSquareFigureTestAsync()
        {
            var body = new
            {
                Type = 0,
                Center = new Point2D
                {
                    X = 2,
                    Y = 2
                },
                Radius = 1
            };

            // ACT
            var setResult = await PostAsync($"api/figure", GetHttpContentJsonBody(body));
            setResult.StatusCode.Should().Be(StatusCodes.Status200OK, "Circle is created");
            
            var answer = await setResult.Content.ReadAsStringAsync();
            var circleId = JsonConvert.DeserializeObject<Guid>(answer);
            circleId.Should().NotBe(Guid.Empty);

            var getResult = await GetAsync($"api/figure/{circleId}");
            getResult.StatusCode.Should().Be(StatusCodes.Status200OK, "Valid figure square");
            var result = await getResult.Content.ReadAsStringAsync();

            var square = JsonConvert.DeserializeObject<double>(result);
            Math.Abs(square - (Math.PI * body.Radius * body.Radius)).Should().BeLessThan(0.000001);
        }
    }
}