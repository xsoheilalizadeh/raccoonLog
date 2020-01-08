using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace raccoonLog.Http.Handlers
{
    public class DefaultHttpRequestLogFormHandler : IHttpRequestLogFormHandler
    {
        private readonly IOptions<RaccoonLogHttpOptions> _options;

        private readonly IDataProtector _dataProtector;

        public DefaultHttpRequestLogFormHandler(IOptions<RaccoonLogHttpOptions> options, IDataProtector dataProtector)
        {
            _options = options;
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

            var formLog = new FormLog();

            var form = await request.ReadFormAsync(cancellationToken);

            var option = _options.Value;

            var sensitiveData = option.SensitiveData.Request.Forms;

            foreach (var item in form)
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                string itemValue;

                if (sensitiveData.TryGetValue(item.Key, out var protectType))
                {
                    itemValue = _dataProtector.Protect(item.Value, protectType);
                }
                else
                {
                    itemValue = item.Value;
                }

                formLog.Form.Add(item.Key, itemValue);
            }

            foreach (var file in form.Files)
            {
                formLog.Files.Add(new FileLog(file));
            }

            logMessage.Body = formLog;
        }
    }
}