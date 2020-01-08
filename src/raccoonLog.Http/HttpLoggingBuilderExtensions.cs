using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using raccoonLog.Http.Handlers;

namespace raccoonLog.Http
{
    public static class HttpLoggingBuilderExtensions
    {
        public static HttpLoggingBuilder AddHttpLogging(this RaccoonLogBuilder builder,
            Action<RaccoonLogHttpOptions> configureOptions)
        {
            var services = builder.Services;

            services.Configure(configureOptions);

            services.AddHttpContextAccessor();

            services.AddScoped<IDataProtector, DataProtector>();

            services.AddScoped<IHttpLoggingProvider, HttpLoggingProvider>();
            services.AddScoped<IHttpLogMessageFactory, HttpLogMessageFactory>();

            services.AddScoped<IHttpLoggingStore, DefaultHttpLoggingStore>();

            // handlers 

            services.AddScoped<IHttpRequestLogFormHandler, DefaultHttpRequestLogFormHandler>();
            services.AddScoped<IHttpMessageLogTraceIdHandler, DefaultHttpMessageLogTraceIdHandler>();

            services.AddScoped<IHttpRequestLogHandler, DefaultHttpRequestLogHandler>();
            services.AddScoped<IHttpResponseLogHandler, DefaultHttpResponseLogHandler>();
            services.AddScoped<IHttpRequestLogBodyHandler, DefaultHttpRequestLogBodyHandler>();
            services.AddScoped<IHttpResponseLogBodyHandler, DefaultHttpResponseLogBodyHandler>();

            return new HttpLoggingBuilder(services);
        }

        public static HttpLoggingBuilder AddHttpLogging(this RaccoonLogBuilder builder)
        {
            return builder.AddHttpLogging(o => { });
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