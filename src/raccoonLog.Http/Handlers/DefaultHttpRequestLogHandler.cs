using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace raccoonLog.Http.Handlers
{
    public class DefaultHttpRequestLogHandler : IHttpRequestLogHandler
    {
        private readonly IHttpRequestLogFormHandler _formContentHandler;

        private readonly IHttpLogMessageFactory _logMessageFactory;

        private readonly IHttpRequestLogBodyHandler _bodyHandler;

        public DefaultHttpRequestLogHandler(IHttpLogMessageFactory logMessageFactory,
            IHttpRequestLogFormHandler formContentHandler,
            IHttpRequestLogBodyHandler bodyHandler)
        {
            _formContentHandler = formContentHandler;
            _logMessageFactory = logMessageFactory;
            _bodyHandler = bodyHandler;
        }

        public async ValueTask<HttpRequestLog> Handle(HttpRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new NullReferenceException(nameof(request));
            }

            request.EnableBuffering();

            var logMessage = await CreateLogMessage(cancellationToken);

            if (logMessage == null)
            {
                throw new NullReferenceException(nameof(logMessage));
            }

            if (request.HasFormContentType)
            {
                await _formContentHandler.Handle(request, logMessage, cancellationToken);
            }
            else
            {
                await _bodyHandler.Handle(request.Body, logMessage, cancellationToken);
            }

            return logMessage;
        }

        private ValueTask<HttpRequestLog> CreateLogMessage(CancellationToken cancellationToken)
        {
            return _logMessageFactory.Create<HttpRequestLog>(cancellationToken);
        }
    }
}