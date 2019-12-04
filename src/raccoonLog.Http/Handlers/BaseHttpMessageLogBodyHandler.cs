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

        public async Task Handle(Stream body, THttpMessageLog logMessage, CancellationToken cancellationToken = default)
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
        }

        protected virtual async ValueTask<object> ReadBodyAsString(Stream body, CancellationToken cancellationToken)
        {
            var reader = PipeReader.Create(body);

            var result = await reader.ReadAsync(cancellationToken);

            string bodyAsString = null;

#if NETCOREAPP3_0
            bodyAsString = Encoding.UTF8.GetString(result.Buffer.FirstSpan);

#elif NETCOREAPP2_2

            bodyAsString = Encoding.UTF8.GetString(result.Buffer.First.ToArray());
#endif
            if (string.IsNullOrEmpty(bodyAsString) || string.IsNullOrWhiteSpace(bodyAsString))
            {
                return null; // this ignores body in json output
            }

            return bodyAsString;
        }

        protected virtual ValueTask<object> DeserializeBody(Stream body, CancellationToken cancellationToken)
        {
            return JsonSerializer.DeserializeAsync<object>(body, cancellationToken: cancellationToken);
        }
    }
}