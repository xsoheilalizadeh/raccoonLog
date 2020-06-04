using System;
using System.Net;

namespace raccoonLog
{
    public class LogContext
    {
        public LogContext(string traceId, HttpRequestLog request, HttpResponseLog response, string protocol)
        {
            TraceId = traceId ?? throw new ArgumentNullException(nameof(traceId));
            Protocol = protocol ?? throw new ArgumentNullException(nameof(protocol));
            Request = request ?? throw new ArgumentNullException(nameof(request));
            Response = response ?? throw new ArgumentNullException(nameof(response));
        }

        public string TraceId { get; private set; }

        public HttpRequestLog Request { get; private set; }

        public HttpResponseLog Response { get; private set; }

        public Exception? Error { get; private set; }

        public string Protocol { get; set; }

        public void SetError(Exception error) => Error = error;


        public override string ToString()
        {
            return string.Format("{0} {1} - {2} {3}", Request.Method, Request.Url, Response.StatusCode, (HttpStatusCode)Response.StatusCode);
        }
    }
}
