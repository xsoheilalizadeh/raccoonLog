using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using raccoonLog.Http.Handlers;

namespace raccoonLog.Http
{
    public class HttpLoggingProvider : IHttpLoggingProvider
    {
        private readonly IHttpRequestLogHandler _requestHandler;
        private readonly IHttpResponseLogHandler _responseHandler;

        private readonly ILogger<HttpRequest> _requestLogger;
        private readonly ILogger<HttpResponse> _responseLogger;

        private readonly IHttpLoggingStore _store;
        private IOptions<RaccoonLogHttpOptions> _options;


        public HttpLoggingProvider(IHttpResponseLogHandler responseHandler,
            IHttpRequestLogHandler requestHandler,
            IOptions<RaccoonLogHttpOptions> options,
            ILoggerFactory loggerFactory,
            IHttpLoggingStore store)
        {
            _store = store;
            _options = options;
            _responseHandler = responseHandler;
            _requestHandler = requestHandler;

            _requestLogger = loggerFactory.CreateLogger<HttpRequest>();
            _responseLogger = loggerFactory.CreateLogger<HttpResponse>();
        }

        public ValueTask LogAsync(HttpRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new NullReferenceException(nameof(request));
            }

            return LogRequest(request, cancellationToken);
        }


        public ValueTask LogAsync(HttpResponse response, Stream body, CancellationToken cancellationToken = default)
        {
            if (response == null)
            {
                throw new NullReferenceException(nameof(response));
            }

            return LogResponse(response, body, cancellationToken);
        }

        private async ValueTask LogResponse(HttpResponse response, Stream body, CancellationToken cancellationToken = default)
        {
            var logMessage = await _responseHandler.Handle(response, body, cancellationToken);

            var options = _options.Value;

            if (options.EnableConsoleLogging)
            {
                var json = JsonSerializer.Serialize(logMessage, options.JsonSerializerOptions);

                _responseLogger.LogInformation(json);
            }

            await _store.StoreAsync(logMessage, cancellationToken);
        }

        private async ValueTask LogRequest(HttpRequest request, CancellationToken cancellationToken = default)
        {
            var logMessage = await _requestHandler.Handle(request, cancellationToken);

            var options = _options.Value;

            if (options.EnableConsoleLogging)
            {
                var json = JsonSerializer.Serialize(logMessage, options.JsonSerializerOptions);

                _requestLogger.LogInformation(json);
            }

            await _store.StoreAsync(logMessage, cancellationToken);
        }
    }
}