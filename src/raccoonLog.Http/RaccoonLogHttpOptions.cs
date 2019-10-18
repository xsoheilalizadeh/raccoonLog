using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace raccoonLog.Http
{
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

        public RaccoonLogHttpRequestOptions Request { get; } = new RaccoonLogHttpRequestOptions();

        public RaccoonLogHttpResponseOptions Response { get; } = new RaccoonLogHttpResponseOptions();

        public JsonSerializerOptions JsonSerializerOptions { get; set; }
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
    }
}       