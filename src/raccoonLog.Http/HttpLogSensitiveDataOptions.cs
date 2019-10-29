using System;
using System.Collections.Generic;
using Microsoft.Net.Http.Headers;

namespace raccoonLog.Http
{
    public class HttpLogSensitiveDataOptions
    {
        public HttpRequestLogSensitiveDataOptions Request { get; } = new HttpRequestLogSensitiveDataOptions();

        public HttpResponseLogSensitiveDataOptions Response { get; } = new HttpResponseLogSensitiveDataOptions();

        public Dictionary<string, ProtectType> Claims { get; set; } = new Dictionary<string, ProtectType>();
    }

    public class HttpMessageLogSensitiveDataOptions
    {
        public Dictionary<string, ProtectType> Headers { get; set; } = new Dictionary<string, ProtectType>();

        public HttpMessageLogBodySensitiveDataOptions Body { get; } = new HttpMessageLogBodySensitiveDataOptions();
    }

    public class HttpRequestLogSensitiveDataOptions : HttpMessageLogSensitiveDataOptions
    {
        public HttpRequestLogSensitiveDataOptions()
        {
            Forms.Add("Password", ProtectType.Encrypt);
            Forms.Add("ConfirmPassword", ProtectType.Encrypt);
            
            Headers.Add(HeaderNames.Authorization, ProtectType.Encrypt);
            Headers.Add(HeaderNames.ProxyAuthorization, ProtectType.Encrypt);
        }

        public Dictionary<string, ProtectType> Parameters { get; set; } = new Dictionary<string, ProtectType>();

        public Dictionary<string, ProtectType> Cookies { get; set; } = new Dictionary<string, ProtectType>();

        public Dictionary<string, ProtectType> Forms { get; set; } = new Dictionary<string, ProtectType>();
    }

    public class HttpResponseLogSensitiveDataOptions : HttpMessageLogSensitiveDataOptions
    {
    }

    public class HttpMessageLogBodySensitiveDataOptions
    {
        [Obsolete("Don't use this because it's not ready.")]
        public Dictionary<string, ProtectType> JsonKeyValues { get; set; } = new Dictionary<string, ProtectType>();
    }

    public enum ProtectType
    {
        Truncate,
        Encrypt
    }
}