using Microsoft.Extensions.DependencyInjection;
using System;
using raccoonLog.Http.Handlers;

namespace raccoonLog.Http
{
    public static class RaccoonLogBuilderExtensions
    {
        public static void AddHttpLogging(this RaccoonLogBuilder builder, Action<RaccoonLogHttpOptions> configureOptions)
        {
            var services = builder.Services;

            services.Configure(configureOptions);

            services.AddHttpContextAccessor();

            services.AddScoped<IHttpLoggingProvider, HttpLoggingProvider>();
            services.AddScoped<IHttpLogMessageFactory, HttpLogMessageFactory>();
            services.AddScoped<IDataProtector, DataProtector>();

            // handlers 

            services.AddScoped<IHttpRequestLogFormHandler, DefaultHttpRequestLogFormHandler>();
            services.AddScoped<IHttpRequestLogAgentHandler, DefaultHttpRequestLogAgentHandler>();
            services.AddScoped<IHttpMessageLogTraceIdHandler, DefaultHttpMessageLogTraceIdHandler>();

            services.AddScoped<IHttpRequestLogHandler, DefaultHttpRequestLogHandler>();
            services.AddScoped<IHttpResponseLogHandler, DefaultHttpResponseLogHandler>();
            services.AddScoped<IHttpRequestLogBodyHandler, DefaultHttpRequestLogBodyHandler>();
            services.AddScoped<IHttpResponseLogBodyHandler, DefaultHttpResponseLogBodyHandler>();
        }

        public static void AddHttpLogging(this RaccoonLogBuilder builder)
        {
            builder.AddHttpLogging(o => { });
        }
    }
}
