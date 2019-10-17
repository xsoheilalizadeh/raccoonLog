using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace raccoonLog.Http
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

        public async Task<HttpResponseLog> Handle(HttpResponse response, Stream body)
        {
            var logMessage = CreateLogMessage();

            if (logMessage == null)
            {
                throw new NullReferenceException(nameof(logMessage));
            }

            if (body == null)
            {
                throw new NullReferenceException(nameof(body));
            }

            logMessage.StatusCode = response.StatusCode;

            logMessage.Status = (HttpStatusCode)response.StatusCode;

            await _bodyHandler.Handle(response.Body, logMessage);

            return logMessage;
        }   

        private HttpResponseLog CreateLogMessage()
        {
            return _logMessageFactory.Create<HttpResponseLog>();
        }
    }
}
