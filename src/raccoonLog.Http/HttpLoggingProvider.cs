using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace raccoonLog.Http
{
    public class HttpLoggingProvider : IHttpLoggingProvider
    {
        private readonly IHttpResponseLogHandler _responseHandler;

        private readonly IHttpRequestLogHandler _requestHandler;

        private readonly ILogger<HttpRequest> _requestLogger;

        private readonly ILogger<HttpResponse> _responseLogger;

        private IOptions<RaccoonLogHttpOptions> _options;

        public HttpLoggingProvider(IHttpResponseLogHandler responseHandler,
            IHttpRequestLogHandler requestHandler,
            IOptions<RaccoonLogHttpOptions> options,
            ILoggerFactory loggerFactory)
        {
            _options = options;
            _responseHandler = responseHandler;
            _requestHandler = requestHandler;
            
            _requestLogger = loggerFactory.CreateLogger<HttpRequest>();
            _responseLogger = loggerFactory.CreateLogger<HttpResponse>();
        }

        public Task Log(HttpRequest request)
        {
            if (request == null)
            {
                throw new NullReferenceException(nameof(request));
            }

            return LogRequest(request);
        }


        public Task Log(HttpResponse response, Stream body)
        {

            if (response == null)
            {
                throw new NullReferenceException(nameof(response));
            }

            return LogResponse(response, body);
        }

        private async Task LogResponse(HttpResponse response, Stream body)
        {
            var logMessage = await _responseHandler.Handle(response, body);

            var options = _options.Value;

            if (options.EnableConsoleLogging)
            {
                var json = JsonSerializer.Serialize(logMessage, options.JsonSerializerOptions);

                _responseLogger.LogInformation(json);
            }

            // store log message
        }

        private async Task LogRequest(HttpRequest request)
        {
            var logMessage = await _requestHandler.Handle(request);

            var options = _options.Value;

            if (options.EnableConsoleLogging)
            {
                var json = JsonSerializer.Serialize(logMessage, options.JsonSerializerOptions);

                _requestLogger.LogInformation(json);
            }

            // store log Message
        }
    }
}
