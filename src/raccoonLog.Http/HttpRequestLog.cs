using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

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

        public AgentLog Agent { get; set; }

        public string Method { get; set; }

        public string Service { get; set; }

        public Dictionary<string, string> Parameters { get; set; }

        public Dictionary<string, string> Cookies { get; set; }

        public void SetUrl(string url, string protocol)
        {
            var uri = new Uri(url);

            Url = new UrlLog(uri, protocol);
        }

        public void SetParameters(IQueryCollection queries)
        {
            foreach (var query in queries)
            {
                Parameters.Add(query.Key, query.Value);
            }
        }

        public void SetCookies(IRequestCookieCollection cookies)
        {
            foreach (var cookie in cookies)
            {
                Cookies.Add(cookie.Key, cookie.Value);
            }
        }
    }
}

