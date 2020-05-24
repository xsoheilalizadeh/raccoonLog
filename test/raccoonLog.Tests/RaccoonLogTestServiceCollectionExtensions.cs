using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using raccoonLog.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace raccoonLog.Tests
{
    public static class RaccoonLogTestServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpLogging(this IServiceCollection services, Action<RaccoonLogHttpOptions> configureOptions = null)
        {
            services.AddRaccoonLog(builder =>
            {
                if (configureOptions != null)
                {
                    builder.AddHttpLogging(configureOptions);

                }
                else
                {
                    builder.AddHttpLogging();
                }
            });

            return services;
        }

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
