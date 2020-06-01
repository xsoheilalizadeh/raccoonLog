using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Moq;
using raccoonLog.Http;
using raccoonLog.Http.Handlers;
using Xunit;

namespace raccoonLog.Tests.Handlers
{

    public class HttpResponseLogHandlerTests
    {
        [Fact]
        public async Task HandleResponseSetsResponseBody()
        {
            var factory = new Mock<IHttpLogMessageFactory>();
            var accessor = new Mock<IHttpContextAccessor>();
            var handler = new DefaultHttpResponseLogHandler(factory.Object, accessor.Object, DefaultOptions.Default);

            var context = new DefaultHttpContext();
            var logMessage = new HttpResponseLog(0, null, null);

            var content = "This the http response content!";
            var response = new FakeHttpResponse(content);
            var bodyWrapper = new HttpResponseBodyWrapper(response.Body);

            context.Features.Set(bodyWrapper);
            context.Features.Set<IHttpResponseFeature>(response);

            accessor.Setup(h => h.HttpContext).Returns(context);
            factory.Setup(f => f.Create(context.Response)).Returns(logMessage);

            await handler.Handle(context.Response);

            bodyWrapper.Dispose();

            Assert.Equal(logMessage.Body, content);
        }
    }

    public class HttpRequestLogHandlerTests
    {
        [Fact]
        public async Task HandleRequestWithFormContentWhenItRequestIsFormContentType()
        {
            var factory = new Mock<IHttpLogMessageFactory>();
            var formContentHandler = new Mock<IHttpRequestLogFormHandler>();
            var handler = new DefaultHttpRequestLogHandler(factory.Object, formContentHandler.Object, DefaultOptions.Default);
            var context = new DefaultHttpContext();

            factory.Setup(f => f.Create(context.Request)).Returns(new HttpRequestLog(null, null, null, null, null, null));

            context.Features.Set<IFormFeature>(new FakeForm());
            context.Features.Set<IHttpRequestFeature>(new FakeHttpRequest());

            await handler.Handle(context.Request);

            formContentHandler.Verify(f => f.Handle(context.Request, It.IsAny<HttpRequestLog>(), CancellationToken.None), Times.Once);
        }


        [Fact]
        public async Task HandleRequestReadBodyContentWhenItIsNotFormContent()
        {
            var factory = new Mock<IHttpLogMessageFactory>();
            var formContentHandler = new Mock<IHttpRequestLogFormHandler>();
            var handler = new DefaultHttpRequestLogHandler(factory.Object, formContentHandler.Object, DefaultOptions.Default);
            var context = new DefaultHttpContext();
            var logMessage = new HttpRequestLog(null, null, null, null, null, null);
            var content = "The Request Body Content!";

            factory.Setup(f => f.Create(context.Request)).Returns(logMessage);

            context.Features.Set<IHttpRequestFeature>(new FakeHttpRequest(content));

            await handler.Handle(context.Request);

            Assert.Equal(logMessage.Body, content);
        }
    }
}