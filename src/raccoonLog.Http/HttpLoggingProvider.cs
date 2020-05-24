using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
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

        private readonly ILogger<HttpLoggingProvider> _logger;

        private readonly IHttpLoggingStore _store;

        private RaccoonLogHttpOptions _options;

        public HttpLoggingProvider(IHttpResponseLogHandler responseHandler,
            IHttpRequestLogHandler requestHandler,
            IOptions<RaccoonLogHttpOptions> options,
            ILogger<HttpLoggingProvider> logger,
            IHttpLoggingStore store)
        {
            _store = store;
            _options = options.Value;
            _responseHandler = responseHandler;
            _requestHandler = requestHandler;
            _logger = logger;
        }

        public async ValueTask LogAsync(HttpContext context, CancellationToken cancellationToken = default)
        {
            var requestLog = await _requestHandler.Handle(context.Request, cancellationToken);

            var responseLog = await _responseHandler.Handle(context.Response, cancellationToken);

            var logContext = new LogContext(context.TraceIdentifier, requestLog, responseLog, context.Request.Protocol);

            var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

            if (exceptionFeature?.Error is Exception error)
            {
                logContext.SetError(error);
            }

            if (_options.EnableConsoleLogging)
            {
                var json = JsonSerializer.Serialize(logContext, _options.JsonSerializerOptions);

                _logger.LogInformation(json);
            }

            await _store.StoreAsync(logContext, cancellationToken);
        }
    }
}