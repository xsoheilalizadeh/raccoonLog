using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace raccoonLog.UnitTests
{
    public class HttpMessageLogMiddlewareTests
    {
        [Fact]
        public async Task InvokeReturnsBackOriginalResponseBodyAfterLog()
        {
            var httpLoggingProvider = new Mock<IHttpLoggingProvider>();
            var context = new DefaultHttpContext();
            var content = "This is response content!";

            context.Response.Body = new MemoryStream();
            context.Response.Body.Write(Encoding.UTF8.GetBytes(content));

            var middleware = new HttpMessageLogMiddleware( (ctx) => Task.CompletedTask);

            await middleware.Invoke(context, httpLoggingProvider.Object);

            context.Response.Body.Position = 0;

            var bodyContent = new StreamReader(context.Response.Body).ReadToEnd();

            Assert.Equal(bodyContent, content);

            httpLoggingProvider.Verify(h => h.LogAsync(context, default), Times.Once);
        }

        [Fact]
        public async Task InvokeReturnsBackOriginalResponseBodyWhenNextThrowsException()
        {
            var httpLoggingProvider = new Mock<IHttpLoggingProvider>();
            var context = new DefaultHttpContext();
            var content = "This is response content!";

            context.Response.Body = new MemoryStream();
            context.Response.Body.Write(Encoding.UTF8.GetBytes(content));

            var middleware = new HttpMessageLogMiddleware((ctx) => throw new InvalidOperationException());

            try
            {
                await middleware.Invoke(context, httpLoggingProvider.Object);
            }
            catch
            {
                // ignored
            }

            context.Response.Body.Position = 0;

            var bodyContent = new StreamReader(context.Response.Body).ReadToEnd();

            Assert.Equal(bodyContent, content);

            httpLoggingProvider.Verify(h => h.LogAsync(context, default), Times.Once);
        }
    }
}
