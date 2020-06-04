using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace raccoonLog.UnitTests.Functional
{
    public static class RaccoonLogTestServiceCollectionExtensions
    {
        public static IServiceCollection SetHttpContext(this IServiceCollection services, HttpContext context)
        {
            services.AddSingleton<IHttpContextAccessor>((sp) =>
            {
                return new HttpContextAccessor() { HttpContext = context };
            });

            return services;
        }
    }

}
