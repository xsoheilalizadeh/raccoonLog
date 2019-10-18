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

        public HttpMessageLog()
        {
            Headers = new Dictionary<string, string>();
            Claims = new Dictionary<string, string>();
            CreatedOn = DateTimeOffset.UtcNow;
        }

        public bool HasBody => _hasBody;

        public bool BodyIgnored { get; private set; }
            
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

        internal void SetHeaders(IHeaderDictionary headers, IList<string> ignoreHeaders)
        {
            foreach (var header in headers)
            {
                if (!ignoreHeaders.Contains(header.Key))
                {
                    Headers.Add(header.Key, header.Value);
                }
            }
        }

        internal void SetClaims(IEnumerable<Claim> claims)
        {
            var claimsDictionary = claims.ToDictionary();

            foreach (var claim in claimsDictionary)
            {
                Claims.Add(claim.Key, claim.Value);
            }
        }

        internal void IgnoreBody() => BodyIgnored = true;
    }
}
