using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Net.Http.Headers;
using raccoonLog.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace raccoonLog.Tests.Handlers
{
    public class DefaultHttpLogAgentHandlerTests
    {
        [Fact]
        public async Task HandleThrowsNullReferenceExceptionOnNullRequest()
        {
            // arrange
            var handler = new DefaultHttpRequestLogAgentHandler();

            // act and assert
            await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(null));
        }

        [Fact]
        public async Task HandleReturnsNullOnNoneSetUserAgent()
        {
            // arrange
            var handler = new DefaultHttpRequestLogAgentHandler();
            var context = new DefaultHttpContext();

            // act 
            var agent = await handler.Handle(context.Request);

            // assert
            Assert.Null(agent);
        }


        [Fact]
        public async Task HandleReturnsInitializedUserAgent()
        {
            // arrange  
            var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3919.0 Safari/537.36 Edg/79.0.299.0";
            var handler = new DefaultHttpRequestLogAgentHandler();
            var context = new DefaultHttpContext();

            context.Request.Headers.Add(HeaderNames.UserAgent, userAgent);

            // act 
            var agent = await handler.Handle(context.Request);

            // assert
            Assert.NotNull(agent.UserAgent.Name);
            Assert.NotNull(agent.Os.Name);

            Assert.NotNull(agent.UserAgent.Version);
            Assert.NotNull(agent.Os.Version);

            Assert.Equal(agent.PureUserAgent, userAgent);
        }

    }
}
