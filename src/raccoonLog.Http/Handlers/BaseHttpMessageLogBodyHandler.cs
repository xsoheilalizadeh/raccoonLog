using System;
using System.IO;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("raccoonLog.Tests")]

namespace raccoonLog.Http.Handlers
{
    public class BaseHttpMessageLogBodyHandler<THttpMessageLog> where THttpMessageLog : HttpMessageLog
    {
        protected internal bool Ignored { get; set; }

        public async ValueTask Handle(Stream body, THttpMessageLog logMessage, CancellationToken cancellationToken = default)
        {
            if (body == null)
            {
                throw new NullReferenceException(nameof(body));
            }    

            if (logMessage == null)
            {
                throw new NullReferenceException(nameof(logMessage));
            }

            if (logMessage.HasBody() || logMessage.IsBodyIgnored())
            {
                Ignored = true;
                return;
            }

            body.Position = 0;

            if (logMessage.IsJson())
            {
                logMessage.Body = await DeserializeBody(body,cancellationToken);
            }
            else
            {
                logMessage.Body = await ReadBodyAsString(body,cancellationToken);
            }

            body.Position = 0;
        }

        protected virtual async ValueTask<object> ReadBodyAsString(Stream body, CancellationToken cancellationToken)
        {
            var bodyAsString = await new StreamReader(body).ReadToEndAsync();

            return string.IsNullOrEmpty(bodyAsString) ? null : bodyAsString;
        }

        protected virtual ValueTask<object> DeserializeBody(Stream body, CancellationToken cancellationToken)
        {
            return JsonSerializer.DeserializeAsync<object>(body, cancellationToken: cancellationToken);
        }
    }
}