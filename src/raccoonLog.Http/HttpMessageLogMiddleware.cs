using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IO;
using System;
using System.IO;
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
            var originalBody = context.Response.Body;

            var bodyStream = _recyclableMemoryStreamManager.GetStream();

            context.Response.Body = bodyStream;

            var feature = new ReadResponseBodyFeature(bodyStream);

            context.Features.Set(feature);

            try
            {
                await _next(context);
            }
            finally
            {
                await httpLogging.LogAsync(context, context.RequestAborted);

                await feature.Body.CopyToAsync(originalBody, context.RequestAborted);

                context.Response.Body = originalBody;

                feature.Dispose();
            }
        }
    }

    public class ReadResponseBodyFeature : IDisposable
    {

        private Stream _innerStream;

        public Stream Body
        {
            get { _innerStream.Position = 0; return _innerStream; }
        }

        public ReadResponseBodyFeature(Stream body)
        {
            _innerStream = body;
        }

        public void Dispose()
        {
            _innerStream?.Dispose();
        }
    }
}