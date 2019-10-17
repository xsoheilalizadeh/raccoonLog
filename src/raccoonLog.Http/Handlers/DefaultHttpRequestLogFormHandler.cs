using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace raccoonLog.Http
{
    public class DefaultHttpRequestLogFormHandler : IHttpRequestLogFormHandler
    {
        public async Task Handle(HttpRequest request, HttpRequestLog logMessage)
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

            var form = await request.ReadFormAsync();

            foreach (var item in form)
            {
                formLog.Form.Add(item.Key, item.Value);
            }

            foreach (var file in form.Files)
            {
                formLog.Files.Add(new FileLog(file));
            }

            logMessage.Body = formLog;
        }
    }
}