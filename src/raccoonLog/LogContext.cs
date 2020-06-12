using System;

namespace raccoonLog
{
    public class LogContext
    {
        public LogContext(string traceId, HttpRequestLog request, HttpResponseLog response,
            string protocol, DateTime timestamp, ExceptionLog? error = null)
        {
            TraceId = traceId;
            Request = request;
            Response = response;
            Error = error;
            Protocol = protocol;
            Timestamp = timestamp;
        }

        public string TraceId { get; }

        public HttpRequestLog Request { get; }

        public HttpResponseLog Response { get; }

        public ExceptionLog? Error { get; private set; }

        public string Protocol { get; set; }

        public DateTime Timestamp { get; }

        public void SetError(Exception exception)
        {
            var error = new ExceptionLog(exception.Message, null, exception.StackTrace, exception.Source,
                exception.HelpLink);

            if (Error is null)
            {
                Error = error;
            }
            else if (Error.InnerError is null)
            {
                Error.SetInnerError(error);
            }
            else
            {
                Error.InnerError.SetInnerError(error);
            }

            while (exception.InnerException != null)
            {
                SetError(exception.InnerException);
                exception = exception.InnerException;
            }
        }

        // public override string ToString()
        // {
        //     return string.Format("{0} {1} - {2} {3}", Request.Method, Request.Url, Response.StatusCode,
        //         (HttpStatusCode) Response.StatusCode);
        // }
    }
}