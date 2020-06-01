using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using Moq;
using raccoonLog.Http;
using Xunit;

namespace raccoonLog.Tests
{
    public class HttpLogMessageFactoryTests
    {
        private Mock<IOptions<RaccoonLogHttpOptions>> options = new Mock<IOptions<RaccoonLogHttpOptions>>();

        [Fact]
        public void CreateRequestLogMatchsWithHttpRequest()
        {
            options.Setup(o => o.Value).Returns(new RaccoonLogHttpOptions());

            var factory = new HttpLogMessageFactory(options.Object, NullProtector.Value);
            var context = new DefaultHttpContext();
            var request = new FakeHttpRequest();
            var cookies = new FakeRequestCookies();

            context.Features.Set<IHttpRequestFeature>(request);
            context.Features.Set<IRequestCookiesFeature>(cookies);

            var requestLog = factory.Create(context.Request);

            Assert.Equal(context.Request.Method, requestLog.Method);
            Assert.Equal(context.Request.Headers, requestLog.Headers);
            Assert.Equal(context.Request.Query, requestLog.Parameters);
            Assert.Equal(context.Request.Cookies, requestLog.Cookies);
            Assert.Equal(context.Request.Scheme, requestLog.Url.Scheme);
            Assert.Equal(context.Request.Host.Port, requestLog.Url.Port);
            Assert.Equal(context.Request.Host.Host, requestLog.Url.Host);
            Assert.Equal($"{context.Request.PathBase}{context.Request.Path}", requestLog.Url.Path);
        }

        [Fact]
        public void CreateResponseLogMatchsWithHttpRequest()
        {
            var factory = new HttpLogMessageFactory(DefaultOptions.Default, NullProtector.Value);
            var context = new DefaultHttpContext();
            var response = new FakeHttpResponse();

            context.Features.Set<IHttpResponseFeature>(response);

            var responseLog = factory.Create(context.Response);

            Assert.Equal(context.Response.Headers, responseLog.Headers);
            Assert.Equal(context.Response.StatusCode, responseLog.StatusCode);
            Assert.Equal(context.Response.ContentType, responseLog.ContentType);
        }

        [Fact]
        public void CreateRequestLogProtectsHttpRequestSensitiveData()
        {
            var option = new RaccoonLogHttpOptions();

            option.Request.SensitiveData.Parameters.Add("name");
            option.Request.SensitiveData.Headers.Add("X-Custom");
            option.Request.SensitiveData.Cookies.Add("auth_token");

            options.Setup(o => o.Value).Returns(option);

            var factory = new HttpLogMessageFactory(options.Object, NullProtector.Value);
            var context = new DefaultHttpContext();
            var request = new FakeHttpRequest();
            var cookies = new FakeRequestCookies();

            context.Features.Set<IHttpRequestFeature>(request);
            context.Features.Set<IRequestCookiesFeature>(cookies);

            var requestLog = factory.Create(context.Request);

            Assert.NotEqual(requestLog.Headers.First(h => h.Key == "X-Custom").Value, context.Request.Headers["X-Custom"]);
            Assert.NotEqual(requestLog.Parameters.First(h => h.Key == "name").Value, context.Request.Query["name"]);
            Assert.NotEqual(requestLog.Cookies.First(h => h.Key == "auth_token").Value, context.Request.Cookies["auth_token"]);
        }

        [Fact]
        public void CreateResponseLogProtectsHttpResponseSensitiveData()
        {
            var option = new RaccoonLogHttpOptions();

            option.Response.SensitiveData.Headers.Add("X-Custom");

            options.Setup(o => o.Value).Returns(option);

            var factory = new HttpLogMessageFactory(options.Object, NullProtector.Value);
            var context = new DefaultHttpContext();
            var response = new FakeHttpResponse();

            context.Features.Set<IHttpResponseFeature>(response);

            var responseLog = factory.Create(context.Response);

            Assert.NotEqual(responseLog.Headers.First(h => h.Key == "X-Custom").Value, context.Response.Headers["X-Custom"]);
        }
    }
}
