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

        public int StatusCode { get; }

        public object? Body { get; set; }

        public string? ContentType { get; }

        public List<KeyValuePair<string, string>> Headers { get; }

        internal void SetBody(object? body)
        {
            Body = body;
        }
    }
}