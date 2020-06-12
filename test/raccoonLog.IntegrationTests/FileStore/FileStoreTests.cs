using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using raccoonLog.IntegrationTests.Domain;
using raccoonLog.Stores.File;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace raccoonLog.IntegrationTests.FileStore
{
    public class FileStoreTests : IClassFixture<TestServerFixture<FileStoreStartup>>
    {
        public FileStoreTests(TestServerFixture<FileStoreStartup> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                HandleCookies = false
            });

            var options = factory.Services.GetService<IOptions<FileStoreOptions>>().Value;
            _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), options.SavePath, options.FileName);

            if (File.Exists(_logFilePath)) File.Delete(_logFilePath);
        }

        private readonly HttpClient _client;

        private readonly string _logFilePath;

        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        private async Task AssertStoredLog(HttpResponseMessage response)
        {
            await Task.Delay(500);

            await using var logFileStream = File.OpenRead(_logFilePath);

            var logContext = (await JsonSerializer.DeserializeAsync<LogContext[]>(logFileStream, SerializerOptions))
                .Single();

            logContext.Request.ShouldBe(response.RequestMessage);

            logContext.Response.ShouldBe(response);

            Assert.Equal(logContext.Protocol, $"HTTP/{response.Version}");
        }

        [Fact]
        public async Task GetJsonRequestStoresInFile()
        {
            var response = await _client.GetAsync(EndPoints.GetJson);

            await AssertStoredLog(response);
        }

        [Fact]
        public async Task GetPlainRequestStoresInFile()
        {
            var response = await _client.GetAsync(EndPoints.GetPlain);

            await AssertStoredLog(response);
        }

        [Fact]
        public async Task PostJsonRequestStoresInFile()
        {
            var json = new StringContent(JsonSerializer.Serialize(Person.Default, SerializerOptions),
                Encoding.UTF8, MediaTypeNames.Application.Json);

            var response = await _client.PostAsync(EndPoints.PostJson, json);

            await AssertStoredLog(response);
        }
    }
}