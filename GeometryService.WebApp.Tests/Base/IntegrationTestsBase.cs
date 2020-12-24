using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GeometryService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GeometryService.WebApp.Tests.Base
{
    /// <summary>
    /// Base class for controller testing
    /// </summary>
    public class IntegrationTestsBase
    {
        private HttpClient _client;
        protected HostAppFactory Factory;
        
        [OneTimeSetUp]
        protected virtual async Task OneTimeSetUpAsync()
        {
            Factory = new HostAppFactory();
            _client = Factory.CreateClient();
            await CreateDatabaseAsync();
        }

        [SetUp]
        protected virtual Task SetUpAsync()
        {
            return Task.CompletedTask;
        }

        [TearDown]
        protected virtual Task TearDownAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///     Метод, выполняющийся после тестов.
        /// </summary>
        /// <remarks>
        ///     Удаление тестовой БД.
        /// </remarks>
        /// <returns> Таск. </returns>
        [OneTimeTearDown]
        protected async Task OneTimeTearDownAsync()
        {
            Factory.Dispose();
        }
        
        protected async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await SendRequestAsync(HttpMethod.Get, url);
        }
        
        protected async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            return await SendRequestAsync(HttpMethod.Post, url, content);
        }
        
        protected async Task<HttpResponseMessage> PutAsync(string url, HttpContent content)
        {
            return await SendRequestAsync(HttpMethod.Put, url, content);
        }
        
        protected async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            return await SendRequestAsync(HttpMethod.Delete, url);
        }
        
        protected StringContent GetHttpContentJsonBody<T>(T payload)
        {
            return new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        }

        private async Task<HttpResponseMessage> SendRequestAsync(HttpMethod method, string url,
            HttpContent content = null)
        {
            var requestMessage = new HttpRequestMessage(method, url);

            if (content != null)
            {
                requestMessage.Content = content;
            }

            return await _client.SendAsync(requestMessage);
        }

        private async Task CreateDatabaseAsync()
        {
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
        }
    }
}