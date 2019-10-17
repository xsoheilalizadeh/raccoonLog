using Microsoft.AspNetCore.Http;
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
