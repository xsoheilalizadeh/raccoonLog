using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace raccoonLog.Http
{
    public class HttpRequestLog
    {
        public HttpRequestLog(UrlLog url, string method,
           List<KeyValuePair<string, StringValues>> parameters,
           List<KeyValuePair<string, StringValues>> headers,
           List<KeyValuePair<string, string>> cookies,
           string? contentType = null
           )
        {
            Url = url;
            Method = method;
            Cookies = cookies;
            Headers = headers;
            Parameters = parameters;
        }

        public UrlLog Url { get; private set; }

        public string Method { get; private set; }

        public object? Body { get; private set; }

        public string? ContentType { get; private set; }

        public IReadOnlyList<KeyValuePair<string, StringValues>> Headers { get; set; }

        public IReadOnlyList<KeyValuePair<string, StringValues>> Parameters { get; private set; }

        public IReadOnlyList<KeyValuePair<string, string>> Cookies { get; private set; }

        internal void SetBody(object? body) => Body = body;
    }
}

