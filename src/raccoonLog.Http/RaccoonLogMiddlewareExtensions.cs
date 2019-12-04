using System;
using Microsoft.AspNetCore.Builder;

namespace raccoonLog.Http
{
    public static class RaccoonLogMiddlewareExtensions
    {
        public static void UseRaccoonLog(this IApplicationBuilder app,
            Action<HttpMessageLogMiddlewareBuilder> configure)
        {
            var builder = new HttpMessageLogMiddlewareBuilder();

            configure(builder);

            if (builder.EnableHttpLogging)
            {
                app.UseMiddleware<HttpMessageLogMiddleware>();
            }
        }

        public static void EnableHttpLogging(this HttpMessageLogMiddlewareBuilder builder)
        {
            builder.EnableHttpLogging = true;
        }
    }
}