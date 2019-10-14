using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IO;
using System;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public class HttpMessageLogMiddelware
    {
        private readonly RequestDelegate _next;

        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        public HttpMessageLogMiddelware(RequestDelegate next)
        {
            _next = next;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context, IHttpLoggingProvider httpLogging)
        {
            await httpLogging.Log(context.Request);

            var originalBody = context.Response.Body;

            using (var bodyStream = _recyclableMemoryStreamManager.GetStream())
            {
                context.Response.Body = bodyStream;

                await _next(context);

                bodyStream.Position = 0;

                await bodyStream.CopyToAsync(originalBody);

                await httpLogging.Log(context.Response, bodyStream);
            }

            context.Response.Body = originalBody;
        }
    }

    public static class RaccoonLogMiddelwareExtensions
    {
        public static void UseRacconLog(this IApplicationBuilder app, Action<HttpMessageLogMiddelwareBuilder> confgureBuilder)
        {
            var builder = new HttpMessageLogMiddelwareBuilder();

            confgureBuilder(builder);

            if (builder.EnableHttpLogging)
            {
                app.UseMiddleware<HttpMessageLogMiddelware>();
            }
        }

        public static void EnableHttpLogging(this HttpMessageLogMiddelwareBuilder builder)
        {
            builder.EnableHttpLogging = true;
        }
    }
}
