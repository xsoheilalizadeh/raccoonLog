using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Moq;
using raccoonLog.Handlers;
using raccoonLog.Mocking;
using Xunit;

namespace raccoonLog.UnitTests.Handlers
{
    public class HttpRequestLogHandlerTests
    {
        [Fact]
        public async Task HandleRequestReadBodyContentWhenItIsNotFormContent()
        {
            var factory = new Mock<IHttpLogMessageFactory>();
            var formContentHandler = new Mock<IHttpRequestLogFormHandler>();
            var handler =
                new DefaultHttpRequestLogHandler(factory.Object, formContentHandler.Object, DefaultOptions.Default);
            var context = new DefaultHttpContext();
            var logMessage = new HttpRequestLog(UrlLog.Default, null, null, null, null);
            var content = "The Request Body Content!";

            factory.Setup(f => f.Create(context.Request)).Returns(logMessage);

            context.Features.Set<IHttpRequestFeature>(new FakeHttpRequest(content));

            await handler.Handle(context.Request);

            Assert.Equal(logMessage.Body, content);
        }

        [Fact]
        public async Task HandleRequestWithFormContentWhenItRequestIsFormContentType()
        {
            var factory = new Mock<IHttpLogMessageFactory>();
            var formContentHandler = new Mock<IHttpRequestLogFormHandler>();
            var handler =
                new DefaultHttpRequestLogHandler(factory.Object, formContentHandler.Object, DefaultOptions.Default);
            var context = new DefaultHttpContext();

            factory.Setup(f => f.Create(context.Request))
                .Returns(new HttpRequestLog(UrlLog.Default, null!, null, null!, null!));

            context.Features.Set<IFormFeature>(new FakeForm());
            context.Features.Set<IHttpRequestFeature>(new FakeHttpRequest());

            await handler.Handle(context.Request);

            formContentHandler.Verify(
                f => f.Handle(context.Request, It.IsAny<HttpRequestLog>(), CancellationToken.None), Times.Once);
        }
    }
}