using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using raccoonLog.Http;
using Xunit;

namespace raccoonLog.Tests
{
    public class HttpLogMessageFactoryTest
    {
        private Mock<IOptions<RaccoonLogHttpOptions>> options = new Mock<IOptions<RaccoonLogHttpOptions>>();

        public HttpLogMessageFactoryTest()
        {
            options.SetupGet(o => o.Value).Returns(new RaccoonLogHttpOptions());
        }

        [Fact]
        public void CreateRequestLogMatchsWithHttpRequest()
        {
            var factory = new HttpLogMessageFactory(options.Object, NullProtector.Value);
            var context = new DefaultHttpContext();
            var request = new FakeHttpRequest();

            context.Features.Set<IHttpRequestFeature>(request);

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
            var factory = new HttpLogMessageFactory(options.Object, NullProtector.Value);
            var context = new DefaultHttpContext();
            var response = new FakeHttpResponse();

            context.Features.Set<IHttpResponseFeature>(response);

            var responseLog = factory.Create(context.Response);

            Assert.Equal(context.Response.Headers, responseLog.Headers);
            Assert.Equal(context.Response.StatusCode, responseLog.StatusCode);
            Assert.Equal(context.Response.ContentType, responseLog.ContenType);
        }

        [Fact]
        public void CreateRequestLogProtectsHttpRequestSensitiveData()
        {
            var factory = new HttpLogMessageFactory(options.Object, NullProtector.Value);
            var option = new RaccoonLogHttpOptions();
            var context = new DefaultHttpContext();
            var request = new FakeHttpRequest();

            options.SetupGet(o => o.Value).Returns(option);

            option.Request.SensitiveData.Headers.Add("X-Custom");
            option.Request.SensitiveData.Parameters.Add("name");

            option.Request.SensitiveData.Cookies.Add("auth_token");

            context.Features.Set<IHttpRequestFeature>(request);

            var requestLog = factory.Create(context.Request);

            Assert.NotEqual(requestLog.Headers.First(h => h.Key == "X-Custom").Value, request.Headers["X-Custom"]);
            Assert.NotEqual(requestLog.Parameters.First(h => h.Key == "name").Value, context.Request.Query["name"]);
            Assert.NotEqual(requestLog.Cookies.First(h => h.Key == "auth_token").Value, context.Request.Cookies["auth_token"]);
        }
    }

    class FakeHttpResponse : IHttpResponseFeature
    {
        public FakeHttpResponse()
        {
            StatusCode = StatusCodes.Status200OK;

            Headers = new HeaderDictionary(new Dictionary<string, StringValues>
            {
               {"X-Custom","boo" },
               {"X-Custom-Test","boo" },
            });
        }

        public int StatusCode { get; set; }

        public string ReasonPhrase { get; set; }

        public IHeaderDictionary Headers { get; set; }

        public Stream Body { get; set; }

        public bool HasStarted => true;

        public void OnCompleted(Func<object, Task> callback, object state)
        {
        }

        public void OnStarting(Func<object, Task> callback, object state)
        {
        }
    }

    class FakeHttpRequest : IHttpRequestFeature
    {
        public FakeHttpRequest() => RawTarget = $"{PathBase}{Path}/{QueryString}";

        public string Protocol { get; set; } = "HTTP/1";
        public string Scheme { get; set; } = "https";
        public string Method { get; set; } = "GET";
        public string PathBase { get; set; } = "/some-base-path";
        public string Path { get; set; } = "/some-path";
        public string QueryString { get; set; } = "?name=soheil&age=21";
        public string RawTarget { get; set; }
        public IHeaderDictionary Headers { get; set; } = new HeaderDictionary(new Dictionary<string, StringValues>
        {
            {"X-Custom","noo" },
            {HeaderNames.Host,"ex.com:7888" }
        });

        public Stream Body { get; set; }
    }

    class NullProtector : IDataProtector
    {
        public static IDataProtector Value = new NullProtector();

        public string Protect(string value)
        {
            return default;
        }
    }
}