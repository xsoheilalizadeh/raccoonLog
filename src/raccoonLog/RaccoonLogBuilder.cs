using Microsoft.Extensions.DependencyInjection;
using raccoonLog.Http;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace raccoonLog
{
    public class RaccoonLogBuilder
    {
        public RaccoonLogBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; set; }

        public void AddHttpLogging(Action<RaccoonLogHttpOptions> configureOptions)
        {
            Services.Configure(configureOptions);

            Services.AddHttpContextAccessor();

            Services.AddScoped<IHttpLoggingProvider, HttpLoggingProvider>();
            Services.AddScoped<IHttpLogMessageFactory, HttpLogMessageFactory>();
            Services.AddScoped<IDataProtector, DataProtector>();

            // handlers 

            Services.AddScoped<IHttpRequestLogFormHandler, DefaultHttpRequestLogFormHandler>();
            Services.AddScoped<IHttpRequestLogAgentHandler, DefaultHttpRequestLogAgentHandler>();
            Services.AddScoped<IHttpMessageLogTraceIdHandler, DefaultHttpMessageLogTraceIdHandler>();

            Services.AddScoped<IHttpRequestLogHandler, DefaultHttpRequestLogHandler>();
            Services.AddScoped<IHttpResponseLogHandler, DefaultHttpResponseLogHandler>();
            Services.AddScoped<IHttpRequestLogBodyHandler, DefaultHttpRequestLogBodyHandler>();
            Services.AddScoped<IHttpResponseLogBodyHandler, DefaultHttpResponseLogBodyHandler>();
        }

        public void AddHttpLogging()
        {
            this.AddHttpLogging(o => { });
        }
    }
}
