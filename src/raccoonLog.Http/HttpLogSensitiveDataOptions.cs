using System;
using System.Collections.Generic;
using System.Text;

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
        public Dictionary<string, ProtectType> Parameters { get; set; } = new Dictionary<string, ProtectType>();

        public Dictionary<string, ProtectType> Cookies { get ; set ; } = new Dictionary<string, ProtectType>();

        public Dictionary<string, ProtectType> Forms { get; set; } = new Dictionary<string, ProtectType>();
    }

    public class HttpResponseLogSensitiveDataOptions : HttpMessageLogSensitiveDataOptions { }

    public class HttpMessageLogBodySensitiveDataOptions
    {
        public Dictionary<string, ProtectType> JsonKeyValues { get; set; } = new Dictionary<string, ProtectType>();
    }

    public enum ProtectType
    {
        Truncate,
        Encrypt
    }
}
