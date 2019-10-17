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
            IgnoreHeaders = new List<string>();
            TraceIdHeaderName = "X-RaccoonLog-Id";
            JsonSerializerOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
            };

            JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        public string TraceIdHeaderName { get; set; }

        public bool EnableConsoleLogging { get; set; }

        public IList<string> IgnoreHeaders { get; set; }

        public JsonSerializerOptions JsonSerializerOptions { get; set; }
    }
}