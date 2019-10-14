using System;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace raccoonLog.Http
{
    public class DefaultHttpLogRequestBodyHandler : IHttpLogRequestBodyHandler
    {
        public async Task Handle(HttpRequest request, HttpRequestLog logMessage)
        {
            if (!logMessage.HasBody())
            {
                if (request.Body.Length > 0)
                {
                    logMessage.Body = logMessage.IsJson() ? await Deserialize(request.Body) : await ReadBodyAsString(request);
                }
            }
        }

        private async Task<string> ReadBodyAsString(HttpRequest request)
        {
            var reader = PipeReader.Create(request.Body);

            var result = await reader.ReadAsync();

            return Encoding.UTF8.GetString(result.Buffer.First.ToArray());
        }

        private ValueTask<object> Deserialize(Stream requestBody)
        {
            return JsonSerializer.DeserializeAsync<object>(requestBody);
        }
    }
}
