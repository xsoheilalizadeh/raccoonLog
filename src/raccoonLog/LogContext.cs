using System;
using System.Net;

namespace raccoonLog
{
    public class ExceptionLog
    {
        public ExceptionLog(string message, ExceptionLog innerError, string? stackTrace, string? source,
            string? helpLink)
        {
            Message = message;
            InnerError = innerError;
            StackTrace = stackTrace;
            Source = source;
            HelpLink = helpLink;
        }

        public string Message { get; private set; }

        public ExceptionLog InnerError { get; private set; }

        public string? StackTrace { get; set; }

        public string? Source { get; set; }

        public string? HelpLink { get; set; }

        public void SetInnerError(ExceptionLog error) => InnerError = error;
    }

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

        public string TraceId { get; private set; }

        public HttpRequestLog Request { get; private set; }

        public HttpResponseLog Response { get; private set; }

        public ExceptionLog? Error { get; private set; }

        public string Protocol { get; set; }
        
        public DateTime Timestamp { get; private set; }

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