using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using raccoonLog.IntegrationTests.FileStore;

namespace raccoonLog.IntegrationTests
{
    public class TestServerFixture<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureLogging(logging => { logging.ClearProviders(); });

            return builder;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseContentRoot(Directory.GetCurrentDirectory());
            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseStartup<FileStoreStartup>();

            base.ConfigureWebHost(builder);
        }
    }
}