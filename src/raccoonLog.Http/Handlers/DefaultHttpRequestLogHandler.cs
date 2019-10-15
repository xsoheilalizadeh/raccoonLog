using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public class DefaultHttpRequestLogHandler : IHttpRequestLogHandler
    {   
        private readonly IHttpRequestLogFormHandler _formContentHandler;

        private readonly IHttpLogMessageFactory _logMessageFactory;

        private readonly IHttpRequestLogBodyHandler _bodyHandler;

        private readonly IHttpRequestLogAgentHandler _logAgentHandler;

        public DefaultHttpRequestLogHandler(IHttpLogMessageFactory logMessageFactory,
            IHttpRequestLogFormHandler formContentHandler,
            IHttpRequestLogBodyHandler bodyHandler,
            IHttpRequestLogAgentHandler logAgentHandler)
        {
            _formContentHandler = formContentHandler;
            _logMessageFactory = logMessageFactory;
            _logAgentHandler = logAgentHandler;
            _bodyHandler = bodyHandler;
        }

        public async Task<HttpRequestLog> Handle(HttpRequest request)
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
                await _bodyHandler.Handle(request.Body, logMessage);
            }

            return logMessage;
        }


        private HttpRequestLog CreateLogMessage()
        {
            return _logMessageFactory.Create<HttpRequestLog>();
        }
    }
}
