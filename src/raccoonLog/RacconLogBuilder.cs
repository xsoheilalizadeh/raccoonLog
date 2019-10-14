using Microsoft.Extensions.DependencyInjection;
using raccoonLog.Http;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace raccoonLog
{
    public class RacconLogBuilder
    {
        public RacconLogBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; set; }

        public void AddHttpLogging(Action<RacconLogHttpOptions> configureOptions)
        {
            Services.Configure(configureOptions);

            Services.AddHttpContextAccessor();

            Services.AddScoped<IHttpLoggingProvider, HttpLoggingProvider>();
            Services.AddScoped<IHttpLogMessageFactory, HttpLogMessageFactory>();

            // handlers 

            Services.AddScoped<IHttpLogFormHandler, DefaultHttpLogFormHandler>();
            Services.AddScoped<IHttpLogAgentHandler, DefaultHttpLogAgentHandler>();
            Services.AddScoped<IHttpLogTraceIdHandler, DefaultHttpLogTraceIdHandler>();

            Services.AddScoped<IHttpLogRequestHandler, DefaultHttpLogRequestHandler>();
            Services.AddScoped<IHttpLogResponseHandler, DefaultHttpLogResponseHandler>();
            Services.AddScoped<IHttpLogRequestBodyHandler, DefaultHttpLogRequestBodyHandler>();
            Services.AddScoped<IHttpLogResponseBodyHandler, DefaultHttpLogResponseBodyHandler>();
        }

        public void AddHttpLogging()
        {
            this.AddHttpLogging(options =>
            {
                options.TraceIdHeaderName = "X-RaccongLog-Id";
                options.JsonSerializerOptions = new JsonSerializerOptions
                {
                    IgnoreNullValues = true,
                };

                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        }
    }
}
