using System;
using System.Collections.Generic;
using Microsoft.Net.Http.Headers;

namespace raccoonLog.Http
{
    public class HttpMessageLogSensitiveDataOptions
    {
        public List<string> Headers { get; set; } = new List<string>();

    }

    public class HttpRequestLogSensitiveDataOptions : HttpMessageLogSensitiveDataOptions
    {
        public HttpRequestLogSensitiveDataOptions()
        {
            Forms.Add("Password");
            Forms.Add("ConfirmPassword");
            
            Headers.Add(HeaderNames.Authorization);
            Headers.Add(HeaderNames.ProxyAuthorization);
        }

        public List<string> Parameters { get; set; } = new List<string>();

        public List<string> Cookies { get; set; } = new List<string>();

        public List<string> Forms { get; set; } = new List<string>();
    }

    public class HttpResponseLogSensitiveDataOptions : HttpMessageLogSensitiveDataOptions
    {
    }
}