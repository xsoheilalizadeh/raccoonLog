using Microsoft.AspNetCore.Http;
using Microsoft.IO;
using System.Threading.Tasks;

namespace raccoonLog
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
            context.Request.EnableBuffering();
   
            var originalBody = context.Response.Body;

            var bodyStream = _recyclableMemoryStreamManager.GetStream();

            context.Response.Body = bodyStream;

            var responseBodyWrapper = new HttpResponseBodyWrapper(bodyStream);

            context.Features.Set(responseBodyWrapper);

            try
            {
                await _next(context);
            }
            finally
            {
                await httpLogging.LogAsync(context, context.RequestAborted);

                await responseBodyWrapper.Body.CopyToAsync(originalBody, context.RequestAborted);

                context.Response.Body = originalBody;

                responseBodyWrapper.Dispose();
            }
        }
    }
}
