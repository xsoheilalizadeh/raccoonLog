using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace raccoonLog.Http.Handlers
{
    public class DefaultHttpResponseLogHandler : IHttpResponseLogHandler
    {
        private readonly IHttpLogMessageFactory _logMessageFactory;

        private readonly IHttpResponseLogBodyHandler _bodyHandler;

        public DefaultHttpResponseLogHandler(IHttpLogMessageFactory logMessageFactory,
            IHttpResponseLogBodyHandler bodyHandler)
        {
            _logMessageFactory = logMessageFactory;
            _bodyHandler = bodyHandler;
        }

        public async ValueTask<HttpResponseLog> Handle(HttpResponse response, Stream body,
            CancellationToken cancellationToken = default)
        {
            if (response == null)
            {
                throw new NullReferenceException();
            }

            if (body == null)
            {
                throw new NullReferenceException(nameof(body));
            }


            var logMessage = await CreateLogMessage(cancellationToken);

            if (logMessage == null)
            {
                throw new NullReferenceException(nameof(logMessage));
            }

            logMessage.StatusCode = response.StatusCode;

            logMessage.Status = (HttpStatusCode) response.StatusCode;

            await _bodyHandler.Handle(response.Body, logMessage, cancellationToken);

            return logMessage;
        }

        private ValueTask<HttpResponseLog> CreateLogMessage(CancellationToken cancellationToken)
        {
            return _logMessageFactory.Create<HttpResponseLog>(cancellationToken);
        }
    }
}