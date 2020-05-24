using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace raccoonLog.Http.Handlers
{
    public class HttpMessageLogBodyReader
    {
        private readonly List<string> ignoredContentTypes;

        public HttpMessageLogBodyReader(List<string> ignoredContentTypes)
        {
            this.ignoredContentTypes = ignoredContentTypes;
        }

        public  ValueTask<object> ReadAsync(Stream body, string contentType, CancellationToken cancellationToken = default)
        {
            if (body == null)
            {
                throw new NullReferenceException(nameof(body));
            }    

            if(body.Length <= 0)
            {
                return default;
            }

            body.Position = 0;

            if (contentType.Equals("application/json; charset=utf-8"))
            {
                return DeserializeBody(body,cancellationToken);
            }
            else
            {
                return ReadBodyAsString(body,cancellationToken);
            }
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