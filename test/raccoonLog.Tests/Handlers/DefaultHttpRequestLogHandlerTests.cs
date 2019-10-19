using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using raccoonLog.Http;
using Xunit;

namespace raccoonLog.Tests.Handlers
{
    public class DefaultHttpRequestLogHandlerTests
    {
        private Mock<IHttpLogMessageFactory> _logMessageFactory;

        private Mock<IHttpRequestLogFormHandler> _formContentHandler;

        private Mock<IHttpRequestLogBodyHandler> _bodyHandler;

        private Mock<IHttpRequestLogAgentHandler> _agentHandler;

        public DefaultHttpRequestLogHandlerTests()
        {
            _bodyHandler = new Mock<IHttpRequestLogBodyHandler>();
            _agentHandler = new Mock<IHttpRequestLogAgentHandler>();
            _formContentHandler = new Mock<IHttpRequestLogFormHandler>();
            _logMessageFactory = new Mock<IHttpLogMessageFactory>();
        }


        [Fact]
        public async Task HanleThrowsNullReferenceExceptionOnNullRequest()
        {
            // arrange
            var logMessage = new HttpRequestLog();
            var handler = CreateHandler();

            // act and assert
            await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(null));
        }

        [Fact]
        public async Task HandleThrowsNullReferenceExceptionOnNullLogMessage()
        {
            // arrange
            var context = new DefaultHttpContext();
            var handler = CreateHandler();

            // act and assert
            await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(context.Request));
        }

        [Fact]
        public async Task HandleInvokeFormContentHandlerOnFormContentRequest()
        {
            // arrange
            var handler = CreateHandler();
            var logMessage = new HttpRequestLog();
            var context = new DefaultHttpContext
            {
                Request =
                {
                    Scheme = "http",
                    Host = new HostString("ex.com"),
                    ContentType = "application/x-www-form-urlencoded"
                }
            };

            _logMessageFactory.Setup(s => s.Create<HttpRequestLog>())
            .ReturnsAsync(logMessage);

            // act 
            await handler.Handle(context.Request);

            // assert (verify) 
            _formContentHandler.Verify(s => s.Handle(context.Request, logMessage), Times.Once);

            _bodyHandler.Verify(s => s.Handle(context.Request.Body, logMessage), Times.Never);
        }


        [Fact]
        public async Task HandleInvokeBodyHandlerOnNormalRequest()
        {
            // arrange
            var handler = CreateHandler();
            var logMessage = new HttpRequestLog();
            var context = new DefaultHttpContext
            {
                Request =
                {
                    Scheme = "http",
                    Host = new HostString("http://ex.com"),
                    ContentType = "application/json"
                }
            };

            _logMessageFactory.Setup(s => s.Create<HttpRequestLog>())
            .ReturnsAsync(logMessage);

            // act 
            await handler.Handle(context.Request);

            // assert (verify) 
            _bodyHandler.Verify(s => s.Handle(context.Request.Body, logMessage), Times.Once);

            _formContentHandler.Verify(s => s.Handle(context.Request, logMessage), Times.Never);
        }


        private DefaultHttpRequestLogHandler CreateHandler()
        {
            return new DefaultHttpRequestLogHandler(
                _logMessageFactory.Object,
                _formContentHandler.Object,
                _bodyHandler.Object,
                _agentHandler.Object
            );
        }
    }
}
