using System.Collections.Generic;
using Microsoft.Net.Http.Headers;

namespace raccoonLog
{
    public class HttpRequestLogSensitiveDataOptions : HttpMessageLogSensitiveDataOptions
    {
        public HttpRequestLogSensitiveDataOptions()
        {
            Forms.Add("Password");
            Forms.Add("ConfirmPassword");

            Headers.Add(HeaderNames.Authorization);
            Headers.Add(HeaderNames.ProxyAuthorization);
        }

        public HashSet<string> Parameters { get; set; } = new HashSet<string>();

        public HashSet<string> Cookies { get; set; } = new HashSet<string>();

        public HashSet<string> Forms { get; set; } = new HashSet<string>();
    }
}