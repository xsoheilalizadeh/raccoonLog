using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using raccoonLog.Http;
using raccoonLog.Tests.Handlers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace raccoonLog.Tests
{
    public class HttpLogMessageFactoryTests
    {
        [Fact]
        public async Task CreateIntializeMessageLogOnHttpRequestFormContent()
        {
            var context = new DefaultHttpContext();
            var request = context.Request;
            var logMessageFactory = new ServiceCollection()
                .SetHttpContext(context)
                .AddHttpLogging()
                .BuildServiceProvider()
                .GetService<HttpLogMessageFactory>();

            request.ContentType = "application/x-www-form-urlencoded";
            context.Features.Set<IHttpRequestFeature>(new RequestFeatureStub());

            var logMessage = await logMessageFactory.Create<HttpRequestLog>();

            Assert.NotNull(logMessage.ContentType);

            Assert.Equal(logMessage.ContentType, request.ContentType);

            Assert.Equal(logMessage.Headers.Count, request.Headers.Count);
        }

        [Fact]
        public async Task CreateIntializeMessageLogOnHttpResponse()
        {
            var context = new DefaultHttpContext();
            var response = context.Response;
            var logMessageFactory = new ServiceCollection() 
                .SetHttpContext(context)
                .AddHttpLogging()
                .BuildServiceProvider()
                .GetService<HttpLogMessageFactory>();

            context.Features.Set<IHttpResponseFeature>(new IResponseFeatureStub());

            var logMessage = await logMessageFactory.Create<HttpResponseLog>();

            Assert.NotNull(logMessage.ContentType);

            Assert.Equal(logMessage.ContentType, response.ContentType);

            Assert.Equal(logMessage.Headers.Count, response.Headers.Count);
        }


        [Fact]
        public async Task CreateIgnoresRequestBodyWhenContentTypeIsIgnored()
        {
            var context = new DefaultHttpContext();
            var request = context.Request;
            var requestContenType = "application/json";
            var logMessageFactory = new ServiceCollection()
                .SetHttpContext(context)
                .AddHttpLogging(o => o.Request.IgnoreContentTypes.Add(requestContenType))
                .BuildServiceProvider()
                .GetService<HttpLogMessageFactory>();

            request.ContentType = requestContenType;
            context.Features.Set<IHttpRequestFeature>(new RequestFeatureStub());

            var logMessage = await logMessageFactory.Create<HttpRequestLog>();

            Assert.True(logMessage.BodyIgnored);
        }

        [Fact]
        public async Task CreateIgnoresResponseBodyWhenContentTypeIsIgnored()
        {
            var context = new DefaultHttpContext();
            var response = context.Response;  
            var requestContenType = "application/json";
            var logMessageFactory = new ServiceCollection()
                .SetHttpContext(context)
                .AddHttpLogging(o => o.Response.IgnoreContentTypes.Add(requestContenType))
                .BuildServiceProvider()
                .GetService<HttpLogMessageFactory>();

            response.ContentType = requestContenType;
            context.Features.Set<IHttpResponseFeature>(new IResponseFeatureStub());

            var logMessage = await logMessageFactory.Create<HttpResponseLog>();

            Assert.True(logMessage.BodyIgnored);
        }

        [Fact]
        public async Task CreateIgnoresRequestBodyWhenContentTypeIsNotIgnored()
        {
            var context = new DefaultHttpContext();
            var request = context.Request;
            var requestContenType = "application/json";
            var logMessageFactory = new ServiceCollection()
                .SetHttpContext(context)
                .AddHttpLogging()
                .BuildServiceProvider()
                .GetService<HttpLogMessageFactory>();

            request.ContentType = requestContenType;
            context.Features.Set<IHttpRequestFeature>(new RequestFeatureStub());

            var logMessage = await logMessageFactory.Create<HttpRequestLog>();

            Assert.False(logMessage.BodyIgnored);
        }

        [Fact]
        public async Task CreateIgnoresResponseBodyWhenContentTypeIsNotIgnored()
        {
            var context = new DefaultHttpContext();
            var response = context.Response;
            var requestContenType = "application/json";
            var logMessageFactory = new ServiceCollection()
                .SetHttpContext(context)
                .AddHttpLogging()
                .BuildServiceProvider()
                .GetService<IHttpLogMessageFactory>();

            response.ContentType = requestContenType;
            context.Features.Set<HttpLogMessageFactory>(new IResponseFeatureStub());

            var logMessage = await logMessageFactory.Create<HttpResponseLog>();

            Assert.False(logMessage.BodyIgnored);
        }
    }

}
