using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace raccoonLog
{
    public class RaccoonLogHttpOptions
    {
        public RaccoonLogHttpOptions()
        {
            Level = LogLevel.Information;
            HandleTimestamp = () => DateTime.UtcNow;
            JsonSerializerOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };

            JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            Formatter = (state, exception) =>
            {
                var log = new StringBuilder();

                log.Append("* TraceId: ").Append(state.TraceId)
                   .AppendFormat("\r\n> {0} ", state.Request.Method.ToUpper()).Append(state.Request.Url);

                foreach (var (key, value) in state.Request.Headers)
                {
                    log.AppendFormat("\r\n> {0}: {1}", key, value);
                }

                log.Append("\r\n");

                log
                   .AppendFormat("\r\n< {0} {1} {2}", state.Protocol, state.Response.StatusCode, (HttpStatusCode)state.Response.StatusCode);

                foreach (var (key, value) in state.Response.Headers)
                {
                    log.AppendFormat("\r\n< {0}: {1}", key, value);
                }

                return log.ToString();
            };
        }

        public Func<DateTime> HandleTimestamp { get; set; }
        
        public LogLevel Level { get; set; }

        public Func<LogContext, Exception, string> Formatter { get; internal set; }

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
