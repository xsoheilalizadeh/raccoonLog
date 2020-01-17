using Microsoft.AspNetCore.Http;
using Microsoft.IO;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public class HttpMessageLogMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        public HttpMessageLogMiddleware(RequestDelegate next)
        {
            _next = next;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context, IHttpLoggingProvider httpLogging)
        {
            await httpLogging.LogAsync(context.Request, context.RequestAborted);

            var originalBody = context.Response.Body;

            var bodyStream = _recyclableMemoryStreamManager.GetStream();

            context.Response.RegisterForDispose(bodyStream);

            context.Response.Body = bodyStream;

            await _next(context);

            await httpLogging.LogAsync(context.Response, bodyStream, context.RequestAborted);

            bodyStream.Position = 0;

            await bodyStream.CopyToAsync(originalBody, context.RequestAborted);

            context.Response.Body = originalBody;
        }
    }
}