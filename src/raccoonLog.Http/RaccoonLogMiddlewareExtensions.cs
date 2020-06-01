using Microsoft.AspNetCore.Builder;

namespace raccoonLog.Http
{
    public static class RaccoonLogMiddlewareExtensions
    {
        public static void UseHttpLogging(this IApplicationBuilder app)
        {
            app.UseMiddleware<HttpMessageLogMiddleware>();
        }
    }
}
