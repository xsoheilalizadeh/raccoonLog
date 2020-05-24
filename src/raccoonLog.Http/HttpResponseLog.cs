using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Primitives;

namespace raccoonLog.Http
{
    public class HttpResponseLog
    {
        public HttpResponseLog(int statusCode, string contenType, List<KeyValuePair<string, StringValues>> headers)
        {
            StatusCode = statusCode;
            ContenType = contenType;
            Headers = headers;
        }

        public int StatusCode { get; private set; }

        public object? Body { get; private set; }

        public string? ContenType { get; private set; }

        public  List<KeyValuePair<string, StringValues>> Headers { get; private set; }

        internal void SetBody(object? body) => Body = body; 
    }
}

