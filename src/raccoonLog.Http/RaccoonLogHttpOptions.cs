using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace raccoonLog.Http
{
    public class RaccoonLogHttpOptions
    {
        private bool _enableConsoleLogging;

        public RaccoonLogHttpOptions()
        {
            TraceIdHeaderName = "X-RaccoonLog-Id";
            JsonSerializerOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            EnableConsoleLogging = true;
        }

        public string TraceIdHeaderName { get; set; }

        public bool EnableConsoleLogging
        {
            get => _enableConsoleLogging;
            set
            {
                _enableConsoleLogging = value;
                
                if (value)
                {
                    JsonSerializerOptions.WriteIndented = true;
                }
            }
        }

        public JsonSerializerOptions JsonSerializerOptions { get; }

        public RaccoonLogHttpRequestOptions Request { get; } = new RaccoonLogHttpRequestOptions();

        public RaccoonLogHttpResponseOptions Response { get; } = new RaccoonLogHttpResponseOptions();
    }

    public abstract class RaccoonLogHttpMessageOptions
    {
        public List<string> IgnoreHeaders { get; set; } = new List<string>();

        public List<string> IgnoreContentTypes { get; set; } = new List<string>();
    }

    public class RaccoonLogHttpResponseOptions : RaccoonLogHttpMessageOptions
    {
        public RaccoonLogHttpResponseOptions()
        {
            IgnoreContentTypes.Add("text/html; charset=utf-8");
        }

        public HttpResponseLogSensitiveDataOptions SensitiveData { get; } = new HttpResponseLogSensitiveDataOptions();
    }

    public class RaccoonLogHttpRequestOptions : RaccoonLogHttpMessageOptions
    {
        public RaccoonLogHttpRequestOptions()
        {
            IgnoreHeaders.Add(HeaderNames.Cookie);
        }

        public HttpRequestLogSensitiveDataOptions SensitiveData { get; } = new HttpRequestLogSensitiveDataOptions();
    }
}