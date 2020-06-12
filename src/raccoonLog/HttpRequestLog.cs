using System.Collections.Generic;

namespace raccoonLog
{
    public class HttpRequestLog
    {
        public HttpRequestLog(UrlLog url, string method, string? contentType,
            IReadOnlyList<KeyValuePair<string, string>> headers, IReadOnlyList<KeyValuePair<string, string>> cookies)
        {
            Url = url;
            Method = method;
            ContentType = contentType;
            Headers = headers;
            Cookies = cookies;
        }


        public UrlLog Url { get; }

        public string Method { get; }

        public object? Body { get; set; }

        public string? ContentType { get; }

        public IReadOnlyList<KeyValuePair<string, string>> Headers { get; set; }

        public IReadOnlyList<KeyValuePair<string, string>> Cookies { get; }

        internal void SetBody(object? body)
        {
            Body = body;
        }
    }
}