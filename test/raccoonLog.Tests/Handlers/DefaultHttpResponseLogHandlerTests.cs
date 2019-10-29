using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using raccoonLog.Http;
using raccoonLog.Http.Handlers;
using Xunit;

namespace raccoonLog.Tests.Handlers
{
    public class DefaultHttpResponseLogHandlerTests
    {
        private Mock<IHttpLogMessageFactory> _logMessageFactor;
        private Mock<IHttpResponseLogBodyHandler> _bodyHandler;

        public DefaultHttpResponseLogHandlerTests()
        {
            _logMessageFactor = new Mock<IHttpLogMessageFactory>();
            _bodyHandler = new Mock<IHttpResponseLogBodyHandler>();
        }


        [Fact]
        public async Task HandleThrowsNullReferenceExceptionOnNullResponse()
        {
            // arrange
            var body = new MemoryStream();
            var handler = CreateHandler();

            // act and assert
            await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(null, body));
        }

        
        [Fact]
        public async Task HandleThrowsNullReferenceExceptionOnNullBody()
        {
            // arrange
            var context = new DefaultHttpContext();
            var handler = CreateHandler();

            // act and assert
            await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(context.Response,null));
        }


        [Fact]
        public async Task HandleThrowsNullReferenceExceptionOnNullLogMessage()
        {
            // arrange
            var context = new DefaultHttpContext();
            var body = new MemoryStream();
            var handler = CreateHandler();
            _logMessageFactor.Setup(s => s.Create<HttpResponseLog>())
                .Returns(() => null);

            // act and assert
            await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(context.Response, body));
        }


        private DefaultHttpResponseLogHandler CreateHandler()
        {
            return new DefaultHttpResponseLogHandler(
                 _logMessageFactor.Object,
                 _bodyHandler.Object
            );
        }
    }
}
