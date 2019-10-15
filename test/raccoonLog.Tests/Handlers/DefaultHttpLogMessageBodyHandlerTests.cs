using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using raccoonLog.Http;
using Xunit;

namespace raccoonLog.Tests.Handlers
{
    public class DefaultHttpRequestLogBodyHandlerTests : BaseHttpLogMessageBodyHandlerTest<HttpRequestLog>
    {
    }

    public class DefaultHttpResponseLogBodyHandlerTests : BaseHttpLogMessageBodyHandlerTest<HttpResponseLog>
    {
    }
        

    public abstract class BaseHttpLogMessageBodyHandlerTest<THttpMessageLog> where THttpMessageLog : HttpMessageLog, new()
    {
        [Fact]
        public async Task HandleThrowsNullReferenceExceptionOnNullBody()
        {
            // arrange  
            var logMessage = new THttpMessageLog();
            var handler = new BaseHttpMessageLogBodyHandler<THttpMessageLog>();
            
            // act and assert
            await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(null, logMessage));
        }

        [Fact]
        public async Task HandleThrowsNullReferenceExceptionOnNullLogMessage()
        {
            // arrange
            var body = new MemoryStream();
            var handler = new BaseHttpMessageLogBodyHandler<THttpMessageLog>();

            // act and assert
            await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(body, null));
        }

        [Fact]
        public async Task HandleIgnoresWhenLogMessageHasBody()
        {
            // arrange
            var body = new MemoryStream();
            var handler = new BaseHttpMessageLogBodyHandler<THttpMessageLog>();
            var logMessage = new THttpMessageLog();

            logMessage.Body = "boo"; // does set has body true

            // act
            await handler.Handle(body, logMessage);

            // assert
            Assert.True(handler.Ignored);
        }

        [Fact]
        public async Task HandleReadBodyAsStringWhenRequestIsNotJson()
        {
            // arrange
            var handler = new BaseHttpMessageLogBodyHandler<THttpMessageLog>();
            var logMessage = new THttpMessageLog();
            var body = new MemoryStream();
            var text = "Hi";
            var textAsBytes = Encoding.UTF8.GetBytes(text);

            await body.WriteAsync(textAsBytes);
                
            // act
            await handler.Handle(body, logMessage);

            // assert
            Assert.Equal(text, logMessage.Body);
            Assert.IsType<string>(logMessage.Body);
        }


        [Fact]
        public async Task HandleReadBodyAsObjectWhenRequestIsJson()
        {
            // arrange
            var handler = new BaseHttpMessageLogBodyHandler<THttpMessageLog>();
            var logMessage = new THttpMessageLog();
            var body = new MemoryStream();
            var text = "{\"name\":\"soheil\"}";
            var textAsBytes = Encoding.UTF8.GetBytes(text);

            await body.WriteAsync(textAsBytes);

            logMessage.ContentType = MediaTypeNames.Application.Json;

            // act
            await handler.Handle(body, logMessage);

            // assert
            var json = Assert.IsType<JsonElement>(logMessage.Body);

            Assert.Equal(text, json.GetRawText());
        }
    }

}
