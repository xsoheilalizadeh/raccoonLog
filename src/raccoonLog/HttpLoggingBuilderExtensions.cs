using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using raccoonLog.Handlers;

namespace raccoonLog
{
    public class RaccoonLogBuilder
    {
        public RaccoonLogBuilder(IServiceCollection services)
        {
            Services = services;
        }

        internal IServiceCollection Services { get; set; }
    }

    public static class RaccoonLogServiceCollectionExtensions
    {
        public static HttpLoggingBuilder AddHttpLogging(this IServiceCollection services,
            Action<RaccoonLogHttpOptions> configureOptions)
        {
            services.Configure(configureOptions);

            services.AddHttpContextAccessor();

            services.AddScoped<IDataProtector, DataProtector>();

            services.AddScoped<IHttpLoggingProvider, HttpLoggingProvider>();
            services.AddScoped<IHttpLogMessageFactory, HttpLogMessageFactory>();

            services.AddScoped<IHttpLoggingStore, DefaultHttpLoggingStore>();
            services.AddSingleton<IStoreQueue, StoreQueue>();

            services.AddHostedService<StoreQueueConsumer>();

            // handlers 

            services.AddScoped<IHttpRequestLogFormHandler, DefaultHttpRequestLogFormHandler>();

            services.AddScoped<IHttpRequestLogHandler, DefaultHttpRequestLogHandler>();
            services.AddScoped<IHttpResponseLogHandler, DefaultHttpResponseLogHandler>();

            return new HttpLoggingBuilder(services);
        }

        public static HttpLoggingBuilder AddHttpLogging(this IServiceCollection services)
        {
            return services.AddHttpLogging(o => { });
        }   

        public static void AddStore<TStore>(this HttpLoggingBuilder builder,
         ServiceLifetime lifetime = ServiceLifetime.Scoped) where TStore : class, IHttpLoggingStore
        {
            var services = builder.Services;

            services.Add(new[]
            {
                ServiceDescriptor.Describe(typeof(IHttpLoggingStore), typeof(TStore), lifetime),
            });
        }
    }
}
