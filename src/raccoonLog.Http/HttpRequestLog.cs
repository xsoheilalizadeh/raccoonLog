using System;
using System.Collections.Generic;

namespace raccoonLog.Http
{
    public class HttpRequestLog : HttpMessageLog
    {
        public HttpRequestLog()
        {
            Parameters = new Dictionary<string, string>();
            Cookies = new Dictionary<string, string>();
            Type = HttpMessageLogType.Request;
        }
        public string Issuer { get; set; }

        public UrlLog Url { get; set; }

        public string Method { get; set; }

        public Dictionary<string, string> Parameters { get; set; }

        public Dictionary<string, string> Cookies { get; set; }

        public void SetUrl(string url, string protocol)
        {
            var uri = new Uri(url);

            Url = new UrlLog(uri, protocol);
        }
    }
}

