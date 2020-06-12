using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using raccoonLog.Mocking;
using Xunit;

namespace raccoonLog.UnitTests.Functional
{
    public class HttpLoggingTests
    {
        [Fact]
        public async Task LogFormRequest()
        {
            var context = new DefaultHttpContext();
            context.Features.Set<IHttpRequestFeature>(new FakeHttpRequest());
            context.Features.Set<IFormFeature>(new FakeForm());
            context.Features.Set(new HttpResponseBodyWrapper(Stream.Null));

            var services = new ServiceCollection()
                .AddLogging()
                .SetHttpContext(context);

            services.AddHttpLogging()
                .AddStore<InMemoryStore>(ServiceLifetime.Singleton);

            var serviceProvider = services.BuildServiceProvider();

            var loggingProvider = serviceProvider.GetService<IHttpLoggingProvider>();

            await loggingProvider.LogAsync(context);
        }

        [Fact]
        public async Task LogGetRequest()
        {
            var context = new DefaultHttpContext();
            context.Features.Set<IHttpRequestFeature>(new FakeHttpRequest());
            context.Features.Set(new HttpResponseBodyWrapper(Stream.Null));

            var services = new ServiceCollection()
                .AddLogging()
                .SetHttpContext(context);

            services.AddHttpLogging()
                .AddStore<InMemoryStore>(ServiceLifetime.Singleton);

            var serviceProvider = services.BuildServiceProvider();

            var loggingProvider = serviceProvider.GetService<IHttpLoggingProvider>();

            await loggingProvider.LogAsync(context);
        }
    }
}