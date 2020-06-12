using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace raccoonLog.Handlers
{
    public class HttpMessageLogBodyReader
    {
        private readonly HashSet<string> _ignoredContentTypes;

        public HttpMessageLogBodyReader(HashSet<string> ignoredContentTypes)
        {
            _ignoredContentTypes = ignoredContentTypes;
        }

        public async ValueTask<object?> ReadAsync(Stream body, string contentType, long? contentLength,
            CancellationToken cancellationToken = default)
        {
            if (body == null) throw new NullReferenceException(nameof(body));

            if (_ignoredContentTypes.Any(t =>
                !string.IsNullOrEmpty(contentType) && t.IndexOf(contentType, StringComparison.Ordinal) > -1))
                return default;

            if (body.Length <= 0) return default;

            bool IsJson()
            {
                return contentType.IndexOf("json", StringComparison.Ordinal) > -1;
            }

            body.Position = 0;

            if (contentType is object && IsJson())
            {
                return await JsonSerializer.DeserializeAsync<object>(body, cancellationToken: cancellationToken);
            }

            var reader = new StreamReader(body);

            return await reader.ReadToEndAsync();
        }
    }
}