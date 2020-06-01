using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace raccoonLog.Http.Handlers
{
    public class DefaultHttpResponseLogHandler : IHttpResponseLogHandler
    {
        private readonly IHttpLogMessageFactory _logMessageFactory;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly RaccoonLogHttpOptions _options;

        public DefaultHttpResponseLogHandler(IHttpLogMessageFactory logMessageFactory,
             IHttpContextAccessor httpContextAccessor,
             IOptions<RaccoonLogHttpOptions> options)
        {
            _options = options.Value;
            _logMessageFactory = logMessageFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async ValueTask<HttpResponseLog> Handle(HttpResponse response, CancellationToken cancellationToken = default)
        {
            if (response == null)
            {
                throw new NullReferenceException();
            }

            var logMessage = _logMessageFactory.Create(response);

            var context = _httpContextAccessor.HttpContext;

            var bodyWrapper = context.Features.Get<HttpResponseBodyWrapper>();

            var reader = new HttpMessageLogBodyReader(_options.Response.IgnoreContentTypes);

            var body = await reader.ReadAsync(bodyWrapper.Body, response.ContentType);

            logMessage.SetBody(body);

            return logMessage;
        }

    }
}
