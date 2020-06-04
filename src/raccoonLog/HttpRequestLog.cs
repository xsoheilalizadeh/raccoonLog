using System.Collections.Generic;

namespace raccoonLog
{
    public class HttpRequestLog
    {
        public HttpRequestLog(UrlLog url, string method, string? contentType, IReadOnlyList<KeyValuePair<string, string>> headers, IReadOnlyList<KeyValuePair<string, string>> cookies)
        {
            Url = url;
            Method = method;
            ContentType = contentType;
            Headers = headers;
            Cookies = cookies;
        }        
        
        public UrlLog Url { get; private set; }

        public string Method { get; private set; }

        public object? Body { get; set; }

        public string? ContentType { get; private set; }

        public IReadOnlyList<KeyValuePair<string, string>> Headers { get; set; }


        public IReadOnlyList<KeyValuePair<string, string>> Cookies { get; private set; }

        internal void SetBody(object? body) => Body = body;
    }
}

