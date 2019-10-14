using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public class DefaultHttpLogResponseHandler : IHttpLogResponseHandler
    {
        private readonly IHttpLogMessageFactory _logMessageFactory;

        private readonly IHttpLogResponseBodyHandler _bodyHandler;

        public DefaultHttpLogResponseHandler(IHttpLogMessageFactory logMessageFactory,
            IHttpLogResponseBodyHandler bodyHandler)
        {
            _logMessageFactory = logMessageFactory;
            _bodyHandler = bodyHandler;
        }

        public async Task<HttpResponseLog> Hendle(HttpResponse response, Stream bodyStream)
        {
            var logMessage = CreateLogMessage();

            logMessage.StatusCode = response.StatusCode;

            logMessage.Status = (HttpStatusCode)response.StatusCode;

            await _bodyHandler.Handle(response, logMessage, bodyStream);

            return logMessage;
        }

        private HttpResponseLog CreateLogMessage()
        {
            return _logMessageFactory.Create<HttpResponseLog>();
        }
    }
}
