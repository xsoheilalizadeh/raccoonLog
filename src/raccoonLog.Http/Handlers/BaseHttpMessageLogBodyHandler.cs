using System;
using System.IO;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("raccoonLog.Tests")]

namespace raccoonLog.Http
{
    public class BaseHttpMessageLogBodyHandler<THttpMessageLog> where THttpMessageLog : HttpMessageLog
    {
        internal protected bool Ignored { get; set; }

        public async Task Handle(Stream body, THttpMessageLog logMessage)
        {
            if (body == null)
            {
                throw new NullReferenceException(nameof(body));
            }

            if (logMessage == null)
            {
                throw new NullReferenceException(nameof(logMessage));
            }

            if (logMessage.HasBody())
            {
                Ignored = true;
                return;
            }   

            body.Position = 0;

            if (logMessage.IsJson())
            {
                logMessage.Body = await DeserializeBody(body);
            }
            else
            {
                logMessage.Body = await ReadBodyAsString(body);
            }
        }

        protected virtual async ValueTask<object> ReadBodyAsString(Stream body)
        {
            var reader = PipeReader.Create(body);

            var result = await reader.ReadAsync();

            return Encoding.UTF8.GetString(result.Buffer.FirstSpan);
        }

        protected virtual ValueTask<object> DeserializeBody(Stream body)
        {
            return JsonSerializer.DeserializeAsync<object>(body);
        }
    }
}
