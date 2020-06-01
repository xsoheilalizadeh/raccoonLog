using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace raccoonLog
{
    public class HttpResponseLog
    {
        public HttpResponseLog(int statusCode, string contentType, List<KeyValuePair<string, StringValues>> headers)
        {
            StatusCode = statusCode;
            ContentType = contentType;
            Headers = headers;
        }

        public int StatusCode { get; private set; }

        public object? Body { get; private set; }

        public string? ContentType { get; private set; }

        public List<KeyValuePair<string, StringValues>> Headers { get; private set; }

        internal void SetBody(object? body) => Body = body;
    }
}

