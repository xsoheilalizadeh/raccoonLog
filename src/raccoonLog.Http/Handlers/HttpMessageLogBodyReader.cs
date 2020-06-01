using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
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

        public async ValueTask<object?> ReadAsync(Stream body, string contentType, CancellationToken cancellationToken = default)
        {
            if (body == null)
            {
                throw new NullReferenceException(nameof(body));
            }

            if (ignoredContentTypes.Any(t => !string.IsNullOrEmpty(contentType) && t.IndexOf(contentType) > -1))
            {
                return default;
            }

            if (body.Length <= 0)
            {
                return default;
            }

            bool isJson() => contentType.IndexOf(MediaTypeNames.Application.Json) > -1;

            body.Position = 0;

            if (contentType is object && isJson())
            {
                return await JsonSerializer.DeserializeAsync<object>(body, cancellationToken: cancellationToken);
            }
            else
            {
                using var reader = new StreamReader(body);
            
                return await reader.ReadToEndAsync();
            }
        }
    }
}
