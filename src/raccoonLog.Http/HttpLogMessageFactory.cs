using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public class HttpLogMessageFactory : IHttpLogMessageFactory
    {
        private IHttpContextAccessor _httpContextAccessor;

        private IOptions<RaccoonLogHttpOptions> _options;

        private IHttpMessageLogTraceIdHandler _traceIdHandler;

        public HttpLogMessageFactory(IHttpContextAccessor httpContextAccessor,
            IHttpMessageLogTraceIdHandler traceIdHandler,
            IOptions<RaccoonLogHttpOptions> options)
        {
            _httpContextAccessor = httpContextAccessor;
            _traceIdHandler = traceIdHandler;
            _options = options;
        }

        public async Task<THttpMessageLog> Create<THttpMessageLog>() where THttpMessageLog : HttpMessageLog, new()
        {
            var context = _httpContextAccessor.HttpContext;

            var logMessage = new THttpMessageLog();

            await _traceIdHandler.Handle(context, logMessage);

            SetCommonLogProperties(logMessage);

            return logMessage;
        }


        private void SetCommonLogProperties<THttpMessageLog>(THttpMessageLog logMessage) where THttpMessageLog : HttpMessageLog, new()
        {
            var context = _httpContextAccessor.HttpContext;

            var user = context.User;

            var options = _options.Value;

            logMessage.SetClaims(user.Claims);

            if (logMessage is HttpRequestLog)
            {
                var request = context.Request;

                var requestOptions = options.Request;

                var ignoreHeaders = requestOptions.IgnoreHeaders;

                logMessage.ContentType = request.ContentType;

                logMessage.SetHeaders(request.Headers, ignoreHeaders);

                if (requestOptions.IgnoreContentTypes.Contains(request.ContentType))
                {
                    logMessage.IgnoreBody();
                }
            }
            else
            {
                var response = context.Response;

                var responseOptions = options.Response;

                var ignoreHeaders = responseOptions.IgnoreHeaders;

                var ignoreContentTypes = responseOptions.IgnoreContentTypes;

                logMessage.ContentType = response.ContentType;

                logMessage.SetHeaders(response.Headers, ignoreHeaders);

                if (ignoreContentTypes.Contains(response.ContentType))
                {
                    logMessage.IgnoreBody();
                }
            }
        }
    }
}
