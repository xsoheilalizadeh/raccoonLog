using Microsoft.Extensions.Options;
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


        public BaseHttpMessageLogBodyHandler()
        {
        }


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

            if (logMessage.HasBody() || logMessage.IsBodyIgnored())
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

            string bodyAsString;

#if NETCOREAPP3_0
       
            bodyAsString = Encoding.UTF8.GetString(result.Buffer.FirstSpan);

#elif NETCOREAPP2_2

            bodyAsString = Encoding.UTF8.GetString(result.Buffer.First.ToArray());
#endif
            if (string.IsNullOrEmpty(bodyAsString) || string.IsNullOrWhiteSpace(bodyAsString))
            {
                return null; // this ignores body in json output
            }
            else
            {
                return bodyAsString;
            }
        }

        protected virtual async ValueTask<object> DeserializeBody(Stream body)
        {
            // { "numbers":[1, 2] }

            var document = await JsonSerializer.DeserializeAsync<JsonElement>(body);

            var numbers = document.GetProperty("numbers"); // [1, 2]

            if (numbers.GetArrayLength() == 0)
            {
                using var memoryStream = new MemoryStream();

                var utf8Json = new Utf8JsonWriter(memoryStream);

                utf8Json.WriteNull("orders");

                // or

                utf8Json.WriteNullValue();

                // or ...

                numbers.WriteTo(utf8Json);
            }
          

            return testElement;
        }
    }

}
