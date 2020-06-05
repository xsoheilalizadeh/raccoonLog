using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Moq;
using raccoonLog.Handlers;
using raccoonLog.Mocking;
using Xunit;

namespace raccoonLog.UnitTests.Handlers
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
}