using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public class DefaultHttpLogRequestHandler : IHttpLogRequestHandler
    {   
        private readonly IHttpLogFormHandler _formContentHandler;

        private readonly IHttpLogMessageFactory _logMessageFactory;

        private readonly IHttpLogRequestBodyHandler _bodyHandler;

        private readonly IHttpLogAgentHandler _logAgentHandler;

        public DefaultHttpLogRequestHandler(IHttpLogMessageFactory logMessageFactory,
            IHttpLogFormHandler formContentHandler,
            IHttpLogRequestBodyHandler bodyHandler,
            IHttpLogAgentHandler logAgentHandler)
        {
            _formContentHandler = formContentHandler;
            _logMessageFactory = logMessageFactory;
            _logAgentHandler = logAgentHandler;
            _bodyHandler = bodyHandler;
        }

        public async Task<HttpRequestLog> Hendle(HttpRequest request)
        {
            request.EnableBuffering();

            var logMessage = CreateLogMessage();

            logMessage.Method = request.Method;

            logMessage.SetParameters(request.Query);

            logMessage.SetCookies(request.Cookies);

            logMessage.SetUrl(request.GetEncodedUrl(), request.Protocol);

            await _logAgentHandler.Handle(request);

            if (request.HasFormContentType)
            {
                await _formContentHandler.Handle(request, logMessage);
            }
            else
            {
                await _bodyHandler.Handle(request, logMessage);
            }

            return logMessage;
        }


        private HttpRequestLog CreateLogMessage()
        {
            return _logMessageFactory.Create<HttpRequestLog>();
        }
    }
}
