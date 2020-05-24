using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace raccoonLog.Http.Handlers
{
    public class DefaultHttpRequestLogFormHandler : IHttpRequestLogFormHandler
    {
        private readonly RaccoonLogHttpOptions _options;

        private readonly IDataProtector _dataProtector;

        public DefaultHttpRequestLogFormHandler(IOptions<RaccoonLogHttpOptions> options, IDataProtector dataProtector)
        {
            _options = options.Value;
            _dataProtector = dataProtector;
        }

        public async ValueTask Handle(HttpRequest request, HttpRequestLog logMessage, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new NullReferenceException(nameof(request));
            }

            if (logMessage == null)
            {
                throw new NullReferenceException(nameof(logMessage));
            }

            var form = await request.ReadFormAsync(cancellationToken);

            var sensitiveData = _options.Request.SensitiveData.Forms;

            var forms = form.Select(item =>
            {
                if (sensitiveData.Contains(item.Key))
                {
                    return new KeyValuePair<string, string>(item.Key, _dataProtector.Protect(item.Value));
                }
                else
                {
                    return new KeyValuePair<string, string>(item.Key, item.Value);
                }
            }).ToList();

            var files = form.Files.Select(file => new FileLog(file)).ToList();

            var formLog = new FormLog(forms, files);

            logMessage.SetBody(formLog);
        }
    }
}