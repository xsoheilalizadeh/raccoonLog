using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace raccoonLog.Http
{

    public class HttpMessageLog
    {
        private object _body;

        private bool _hasBody;

        private bool _bodyIgnored;
            
        public HttpMessageLog()
        {
            Headers = new Dictionary<string, string>();
            Claims = new Dictionary<string, string>();  
            CreatedOn = DateTimeOffset.UtcNow;
        }
     
        public string TraceId { get; set; } 

        public string ContentType { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public HttpMessageLogType Type { get; set; }

        public Dictionary<string, string> Headers { get; private set; }

        public Dictionary<string, string> Claims { get; private set; }

        public object Body
        {
            get => _body;
            set
            {
                _body = value;

                _hasBody = true;
            }
        }

        public bool HasBody() => _hasBody;

        public bool IsBodyIgnored() => _bodyIgnored;

        internal void IgnoreBody() => _bodyIgnored = true;
    }
}
