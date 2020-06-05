using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Moq;
using raccoonLog.Mocking;
using Xunit;

namespace raccoonLog.UnitTests
{
    public class HttpLogMessageFactoryTests
    {
        private readonly Mock<IOptions<RaccoonLogHttpOptions>> _options = new Mock<IOptions<RaccoonLogHttpOptions>>();

        [Fact]
        public void CreateRequestLogMatchesWithHttpRequest()
        {
            _options.Setup(o => o.Value).Returns(new RaccoonLogHttpOptions());

            var factory = new HttpLogMessageFactory(_options.Object, NullProtector.Value);
            var context = new DefaultHttpContext();
            var request = new FakeHttpRequest();
            var cookies = new FakeRequestCookies();

            context.Features.Set<IHttpRequestFeature>(request);
            context.Features.Set<IRequestCookiesFeature>(cookies);

            var requestLog = factory.Create(context.Request);

            Assert.Equal(context.Request.Method, requestLog.Method);
            Assert.Equal(context.Request.Headers.Keys, requestLog.Headers.Select(h => h.Key).ToList());
            Assert.Equal(context.Request.Cookies, requestLog.Cookies);
            Assert.Equal(context.Request.Scheme, requestLog.Url.Scheme);
            Assert.Equal(context.Request.Host.Port, requestLog.Url.Port);
            Assert.Equal(context.Request.Host.Host, requestLog.Url.Host);
            Assert.Equal($"{context.Request.PathBase}{context.Request.Path}", requestLog.Url.Path);
        }

        [Fact]
        public void CreateResponseLogMatchesWithHttpRequest()
        {
            var factory = new HttpLogMessageFactory(DefaultOptions.Default, NullProtector.Value);
            var context = new DefaultHttpContext();
            var response = new FakeHttpResponse();

            context.Features.Set<IHttpResponseFeature>(response);

            var responseLog = factory.Create(context.Response);

            Assert.Equal(context.Response.Headers.Keys, responseLog.Headers.Select(h => h.Key).ToList());
            Assert.Equal(context.Response.Headers.Values, responseLog.Headers.Select(h => (StringValues)h.Value).ToList());

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

            _options.Setup(o => o.Value).Returns(option);

            var factory = new HttpLogMessageFactory(_options.Object, NullProtector.Value);
            var context = new DefaultHttpContext();
            var request = new FakeHttpRequest();
            var cookies = new FakeRequestCookies();

            context.Features.Set<IHttpRequestFeature>(request);
            context.Features.Set<IRequestCookiesFeature>(cookies);

            var requestLog = factory.Create(context.Request);

            Assert.NotEqual(requestLog.Headers.First(h => h.Key == "X-Custom").Value, context.Request.Headers["X-Custom"].ToString());
            Assert.NotEqual(requestLog.Cookies.First(h => h.Key == "auth_token").Value, context.Request.Cookies["auth_token"]);
            Assert.NotEqual(requestLog.Url.Parameters.First(h => h.Key == "name").Value, context.Request.Query["name"].ToString());

        }

        [Fact]
        public void CreateResponseLogProtectsHttpResponseSensitiveData()
        {
            var option = new RaccoonLogHttpOptions();

            option.Response.SensitiveData.Headers.Add("X-Custom");

            _options.Setup(o => o.Value).Returns(option);

            var factory = new HttpLogMessageFactory(_options.Object, NullProtector.Value);
            var context = new DefaultHttpContext();
            var response = new FakeHttpResponse();

            context.Features.Set<IHttpResponseFeature>(response);

            var responseLog = factory.Create(context.Response);

            Assert.NotEqual(responseLog.Headers.First(h => h.Key == "X-Custom").Value, context.Response.Headers["X-Custom"].ToString());
        }
    }
}
