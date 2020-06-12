using System;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

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
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            Formatter = (state, exception) =>
            {
                var log = new StringBuilder();

                log.Append("* TraceId: ").Append(state.TraceId)
                    .AppendFormat("\r\n> {0} ", state.Request.Method.ToUpper()).Append(state.Request.Url);

                foreach (var (key, value) in state.Request.Headers) log.AppendFormat("\r\n> {0}: {1}", key, value);

                log.Append("\r\n");

                log
                    .AppendFormat("\r\n< {0} {1} {2}", state.Protocol, state.Response.StatusCode,
                        (HttpStatusCode) state.Response.StatusCode);

                foreach (var (key, value) in state.Response.Headers) log.AppendFormat("\r\n< {0}: {1}", key, value);

                return log.ToString();
            };
        }

        public Func<DateTime> HandleTimestamp { get; set; }

        public LogLevel Level { get; set; }

        public Func<LogContext, Exception, string> Formatter { get; set; }

        public JsonSerializerOptions JsonSerializerOptions { get; }

        public RaccoonLogHttpRequestOptions Request { get; } = new RaccoonLogHttpRequestOptions();

        public RaccoonLogHttpResponseOptions Response { get; } = new RaccoonLogHttpResponseOptions();
    }
}