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
            var ct = context.RequestAborted;

            await httpLogging.LogAsync(context.Request, ct);

            var originalBody = context.Response.Body;

            using var bodyStream = _recyclableMemoryStreamManager.GetStream();

            context.Response.Body = bodyStream;

            await _next(context);

            await httpLogging.LogAsync(context.Response, bodyStream, ct);

            bodyStream.Position = 0;

            await bodyStream.CopyToAsync(originalBody, ct);

            context.Response.Body = originalBody;
        }
    }
}