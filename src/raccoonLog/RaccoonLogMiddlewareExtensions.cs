using Microsoft.AspNetCore.Builder;

namespace raccoonLog
{
    public static class RaccoonLogMiddlewareExtensions
    {
        public static void UseHttpLogging(this IApplicationBuilder app)
        {
            app.UseMiddleware<HttpMessageLogMiddleware>();
        }
    }
}
