using System.Collections.Generic;

namespace raccoonLog
{
    public class HttpResponseLog
    {
        public HttpResponseLog(int statusCode, string contentType, List<KeyValuePair<string, string>> headers)
        {
            StatusCode = statusCode;
            ContentType = contentType;
            Headers = headers;
        }

        public int StatusCode { get; private set; }

        public object? Body { get; set; }

        public string? ContentType { get; private set; }

        public List<KeyValuePair<string, string>> Headers { get; private set; }

        internal void SetBody(object? body) => Body = body;
    }
}

