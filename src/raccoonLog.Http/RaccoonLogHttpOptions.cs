using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace raccoonLog.Http
{
    public class DefaultSensitiveData
    {
        public static IList<string> RequestHeaders = new List<string>
        {
            "Authorization",    
            "Proxy-Authorization"
        };


        public static IList<string> Forms = new List<string>
        {
            "Password",
            "ConfirmPassword"
        };
    }

    public class RaccoonLogHttpOptions
    {
        public RaccoonLogHttpOptions()
        {
            TraceIdHeaderName = "X-RaccoonLog-Id";
            JsonSerializerOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
            };

            JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        public string TraceIdHeaderName { get; set; }

        public bool EnableConsoleLogging { get; set; }

        public JsonSerializerOptions JsonSerializerOptions { get; }
        
        public HttpLogSensitiveDataOptions SensitiveData { get; } = new HttpLogSensitiveDataOptions();


        public RaccoonLogHttpRequestOptions Request { get; } = new RaccoonLogHttpRequestOptions();

        public RaccoonLogHttpResponseOptions Response { get; } = new RaccoonLogHttpResponseOptions();
    }


    public abstract class RaccoonLogHttpMessageOptions
    {
        public IList<string> IgnoreHeaders { get; set; } = new List<string>();

        public IList<string> IgnoreContentTypes { get; set; } = new List<string>();
    }

    public class RaccoonLogHttpResponseOptions : RaccoonLogHttpMessageOptions
    {
    }

    public class RaccoonLogHttpRequestOptions : RaccoonLogHttpMessageOptions
    {
        public RaccoonLogHttpRequestOptions()
        {
            IgnoreHeaders.Add(HeaderNames.Cookie);
            IgnoreContentTypes.Add("text/html; charset=utf-8");
        }
    }
}