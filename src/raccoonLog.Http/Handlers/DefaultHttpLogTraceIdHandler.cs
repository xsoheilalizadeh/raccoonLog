using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace raccoonLog.Http
{
    public class DefaultHttpLogTraceIdHandler : IHttpLogTraceIdHandler
    {
        public IOptions<RacconLogHttpOptions> _options;

        public DefaultHttpLogTraceIdHandler(IOptions<RacconLogHttpOptions> options)
        {
            _options = options;
        }

        public Task Handle(HttpContext context, HttpMessageLog logMessage)
        {
            if (context == null)
            {
                throw new NullReferenceException(nameof(context));
            }

            if (logMessage == null)
            {
                throw new NullReferenceException(nameof(logMessage));
            }

            logMessage.TraceId = context.TraceIdentifier;

            var response = context.Response;

            var options = _options.Value;

            if(!response.HasStarted)
            {
                response.Headers.Add(options.TraceIdHeaderName, logMessage.TraceId);
            }

            return Task.CompletedTask;  
        }
    }
}
