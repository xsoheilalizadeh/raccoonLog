using System.Net;

namespace raccoonLog.Http
{
    public class HttpResponseLog : HttpMessageLog
    {
        public HttpResponseLog()
        {
            Type = HttpMessageLogType.Response;
        }

        public int StatusCode { get; set; }

        public HttpStatusCode Status { get; set; }
    }
}

