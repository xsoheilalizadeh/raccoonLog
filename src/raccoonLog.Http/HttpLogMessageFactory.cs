using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace raccoonLog.Http
{
    public class HttpLogMessageFactory : IHttpLogMessageFactory
    {
        private IHttpContextAccessor _httpContextAccessor;

        private IOptions<RacconLogHttpOptions> _options;

        private IHttpLogTraceIdHandler _traceIdHandler;

        public HttpLogMessageFactory(IHttpContextAccessor httpContextAccessor,
            IHttpLogTraceIdHandler traceIdHandler,
            IOptions<RacconLogHttpOptions> options)
        {
            _httpContextAccessor = httpContextAccessor;
            _traceIdHandler = traceIdHandler;
            _options = options;
        }

        public THttpMessageLog Create<THttpMessageLog>() where THttpMessageLog : HttpMessageLog, new()
        {
            var context = _httpContextAccessor.HttpContext;

            var logMessage = new THttpMessageLog();

            _traceIdHandler.Handle(context, logMessage);

            SetCommonLogProperties(logMessage);

            return logMessage;
        }


        private void SetCommonLogProperties<THttpMessageLog>(THttpMessageLog logMessage) where THttpMessageLog : HttpMessageLog, new()
        {
            var context = _httpContextAccessor.HttpContext;

            var user = context.User;

            var ignoreHeaders = _options.Value.IgnoreHeaders;

            logMessage.SetClaims(user.Claims);

            if (logMessage is HttpRequestLog)
            {
                var request = context.Request;

                logMessage.ContentType = request.ContentType;

                logMessage.SetHeaders(request.Headers, ignoreHeaders);
            }
            else
            {
                var response = context.Response;

                logMessage.ContentType = response.ContentType;

                logMessage.SetHeaders(response.Headers, ignoreHeaders);
            }
        }
    }
}
