using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace raccoonLog.Handlers
{
    public class DefaultHttpRequestLogHandler : IHttpRequestLogHandler
    {
        private readonly IHttpRequestLogFormHandler _formContentHandler;

        private readonly IHttpLogMessageFactory _logMessageFactory;

        private readonly RaccoonLogHttpOptions _options;

        public DefaultHttpRequestLogHandler(IHttpLogMessageFactory logMessageFactory,
            IHttpRequestLogFormHandler formContentHandler,
            IOptions<RaccoonLogHttpOptions> options)
        {
            _options = options.Value;
            _formContentHandler = formContentHandler;
            _logMessageFactory = logMessageFactory;
        }

        public async ValueTask<HttpRequestLog> Handle(HttpRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new NullReferenceException(nameof(request));
            }

            var logMessage = _logMessageFactory.Create(request);

            if (request.HasFormContentType)
            {
                await _formContentHandler.Handle(request, logMessage, cancellationToken);
            }
            else
            {
                var reader = new HttpMessageLogBodyReader(_options.Request.IgnoreContentTypes);

                var body = await reader.ReadAsync(request.Body, request.ContentType, request.ContentLength,
                    cancellationToken);

                logMessage.SetBody(body);
            }

            return logMessage;
        }
    }
}