using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using raccoonLog.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using raccoonLog.Http.Handlers;
using Xunit;

namespace raccoonLog.Tests.Handlers
{
    public class DefaultHttpLogTraceIdHandlerTests
    {

        [Fact]
        public async Task HandleThrowsNullReferenceOnNullContext()
        {
            // arrange
            var options = new StubHttpLogOptions();
            var handler = new DefaultHttpMessageLogTraceIdHandler(options);

            // act and assert
            await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(null, null));
        }

        [Fact]
        public async Task HandleThrowsNullReferenceOnNullLogMessage()
        {
            // arrange
            var options = new StubHttpLogOptions();
            var context = new DefaultHttpContext();
            var handler = new DefaultHttpMessageLogTraceIdHandler(options);

            // act and assert
            await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(context, null));
        }


        [Fact]
        public async Task HandleInitializeTraceId()
        {
            // arrange  
            var options = new StubHttpLogOptions();
            var context = new DefaultHttpContext();
            var logMessage = new HttpMessageLog();
            var handler = new DefaultHttpMessageLogTraceIdHandler(options);

            // act
            await handler.Handle(context, logMessage);

            // assert
            Assert.Equal(context.TraceIdentifier, logMessage.TraceId);
        }

        [Fact]
        public async Task HandleSetsTraceIdHeaderInResponse()
        {
            // arrange      
            var options = new StubHttpLogOptions();
            var context = new DefaultHttpContext();
            var logMessage = new HttpMessageLog();
            var handler = new DefaultHttpMessageLogTraceIdHandler(options);

            // act
            await handler.Handle(context, logMessage);

            // assert
            Assert.Equal(context.Response.Headers[options.Value.TraceIdHeaderName], logMessage.TraceId);
        }

        [Fact]
        public async Task HandleDoesNotSetsTraceIdHeaderWhenResponseHasStarted()
        {
            // arrange              
            var options = new StubHttpLogOptions();
            var context = new DefaultHttpContext();
            var logMessage = new HttpMessageLog();
            var handler = new DefaultHttpMessageLogTraceIdHandler(options);

            context.Features.Set<IHttpResponseFeature>(new StartedResponseFeature());

            // act
            await handler.Handle(context, logMessage);

            // assert
            Assert.Empty(context.Response.Headers[options.Value.TraceIdHeaderName]);
        }
    }


    class StubHttpLogOptions : IOptions<RaccoonLogHttpOptions>
    {
        public RaccoonLogHttpOptions Value => new RaccoonLogHttpOptions { TraceIdHeaderName = "x-trace-id" };
    }

    class StartedResponseFeature : IHttpResponseFeature
    {
        public int StatusCode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ReasonPhrase { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IHeaderDictionary Headers { get; set; } = new HeaderDictionary();
        public Stream Body { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool HasStarted => true;

        public void OnCompleted(Func<object, Task> callback, object state)
        {
            throw new NotImplementedException();
        }

        public void OnStarting(Func<object, Task> callback, object state)
        {
            throw new NotImplementedException();
        }
    }

}
