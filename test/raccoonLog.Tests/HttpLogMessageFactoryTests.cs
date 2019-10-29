using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using raccoonLog.Http;
using System.Threading.Tasks;
using Xunit;

namespace raccoonLog.Tests
{
    public class HttpLogMessageFactoryTests
    {
        [Fact]
        public async Task CreateInitializeMessageLogOnHttpRequestFormContent()
        {
            var context = new DefaultHttpContext();
            var request = context.Request;
            var logMessageFactory = new ServiceCollection()
                .SetHttpContext(context)
                .AddHttpLogging()
                .BuildServiceProvider()
                .GetService<IHttpLogMessageFactory>();

            request.ContentType = "application/x-www-form-urlencoded";
            context.Features.Set<IHttpRequestFeature>(new RequestFeatureStub());

            var logMessage = await logMessageFactory.Create<HttpRequestLog>();

            Assert.NotNull(logMessage.ContentType);

            Assert.Equal(logMessage.ContentType, request.ContentType);

            Assert.Equal(logMessage.Headers.Count, request.Headers.Count);
        }    

        [Fact]
        public async Task CreateInitializeMessageLogOnHttpResponse()
        {
            var context = new DefaultHttpContext();    
            var response = context.Response;
            var logMessageFactory = new ServiceCollection() 
                .SetHttpContext(context)
                .AddHttpLogging()
                .BuildServiceProvider()
                .GetService<IHttpLogMessageFactory>();

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
            var requestContentType = "application/json";
            var logMessageFactory = new ServiceCollection()
                .SetHttpContext(context)
                .AddHttpLogging(o => o.Request.IgnoreContentTypes.Add(requestContentType))
                .BuildServiceProvider()
                .GetService<IHttpLogMessageFactory>();

            request.ContentType = requestContentType;
            context.Features.Set<IHttpRequestFeature>(new RequestFeatureStub());

            var logMessage = await logMessageFactory.Create<HttpRequestLog>();

            Assert.True(logMessage.IsBodyIgnored());
        }

        [Fact]
        public async Task CreateIgnoresResponseBodyWhenContentTypeIsIgnored()
        {
            var context = new DefaultHttpContext();
            var response = context.Response;  
            var requestContentType = "application/json";
            var logMessageFactory = new ServiceCollection()
                .SetHttpContext(context)
                .AddHttpLogging(o => o.Response.IgnoreContentTypes.Add(requestContentType))
                .BuildServiceProvider()
                .GetService<IHttpLogMessageFactory>();

            response.ContentType = requestContentType;
            context.Features.Set<IHttpResponseFeature>(new IResponseFeatureStub());

            var logMessage = await logMessageFactory.Create<HttpResponseLog>();

            Assert.True(logMessage.IsBodyIgnored());
        }

        [Fact]
        public async Task CreateIgnoresRequestBodyWhenContentTypeIsNotIgnored()
        {
            var context = new DefaultHttpContext();
            var request = context.Request;
            var requestContentType = "application/json";
            var logMessageFactory = new ServiceCollection() 
                .SetHttpContext(context)
                .AddHttpLogging()
                .BuildServiceProvider()
                .GetService<IHttpLogMessageFactory>();

            request.ContentType = requestContentType;
            context.Features.Set<IHttpRequestFeature>(new RequestFeatureStub());

            var logMessage = await logMessageFactory.Create<HttpRequestLog>();

            Assert.False(logMessage.IsBodyIgnored());
        }

        [Fact]
        public async Task CreateIgnoresResponseBodyWhenContentTypeIsNotIgnored()
        {
            var context = new DefaultHttpContext();
            var response = context.Response;
            var requestContentType = "application/json";
            var logMessageFactory = new ServiceCollection()
                .SetHttpContext(context)
                .AddHttpLogging()
                .BuildServiceProvider()
                .GetService<IHttpLogMessageFactory>();

            response.ContentType = requestContentType;
            context.Features.Set<IHttpResponseFeature>(new IResponseFeatureStub());

            var logMessage = await logMessageFactory.Create<HttpResponseLog>();

            Assert.False(logMessage.IsBodyIgnored());
        }
    }

}
