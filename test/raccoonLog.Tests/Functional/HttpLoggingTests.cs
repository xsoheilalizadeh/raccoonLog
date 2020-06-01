using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using raccoonLog.Http;
using Xunit;

namespace raccoonLog.Tests.Functional
{
    public class HttpLoggingTests
    {
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

            Assert.Null(InMemoryStore.Context.Error);
            Assert.NotNull(InMemoryStore.Context);
            Assert.NotNull(InMemoryStore.Context.Request);
            Assert.NotNull(InMemoryStore.Context.Response);
            Assert.NotNull(InMemoryStore.Context.Protocol);
            Assert.NotNull(InMemoryStore.Context.TraceId);
        }

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

            Assert.Null(InMemoryStore.Context.Error);
            Assert.NotNull(InMemoryStore.Context);
            Assert.NotNull(InMemoryStore.Context.Request);
            Assert.NotNull(InMemoryStore.Context.Response);
            Assert.NotNull(InMemoryStore.Context.Protocol);
            Assert.NotNull(InMemoryStore.Context.TraceId);
            Assert.True(InMemoryStore.Context.Request.Body is FormLog);
        }
    }

    public class InMemoryStore : IHttpLoggingStore
    {
        public static LogContext Context { get; set; }

        public ValueTask StoreAsync(LogContext logContext, CancellationToken cancellationToken = default)
        {
            Context = logContext;

            return default;
        }
    }

}
