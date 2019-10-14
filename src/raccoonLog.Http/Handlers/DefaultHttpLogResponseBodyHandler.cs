using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace raccoonLog.Http
{
    public class DefaultHttpLogResponseBodyHandler : IHttpLogResponseBodyHandler
    {
        public async Task Handle(HttpResponse response, HttpResponseLog logMessage, Stream bodyStream)
        {
            if (!logMessage.HasBody())
            {
                if (response.Body.Length > 0)
                {
                    logMessage.Body = logMessage.IsJson() ? await Deserialize(bodyStream) : await ReadBodyAsString(bodyStream);
                }
            }
        }

        private async Task<string> ReadBodyAsString(Stream bodyStream)
        {
            PipeReader reader;

            bodyStream.Position = 0;

            reader = PipeReader.Create(bodyStream);   
     
            var result = await reader.ReadAsync();

#if NETCOREAPP3_0
            return Encoding.UTF8.GetString(result.Buffer.FirstSpan);
#else
            return Encoding.UTF8.GetString(result.Buffer.First.ToArray());
#endif  
        }

        private ValueTask<object> Deserialize(Stream requestBody)
        {
            return JsonSerializer.DeserializeAsync<object>(requestBody);
        }
    }
}
