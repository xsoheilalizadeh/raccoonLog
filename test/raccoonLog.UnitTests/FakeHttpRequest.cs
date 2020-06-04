using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace raccoonLog.UnitTests
{
    public class FakeHttpRequest : IHttpRequestFeature
    {
        public FakeHttpRequest(string? body = null)
        {
            RawTarget = $"{PathBase}{Path}/{QueryString}";

            var stream = new MemoryStream();

            if (body is object)
            {
                stream.Write(Encoding.UTF8.GetBytes(body));
            }

            Body = stream;
            Protocol = "HTTP/1";
            Scheme = "https";
            Method = "GET";
            PathBase = "/some-base-path";
            Path = "/some-path";
            QueryString = "?name=soheil&age=21";

            Headers = new HeaderDictionary(new Dictionary<string, StringValues>
            {
                {"X-Custom","noo" },
                {HeaderNames.Host,"ex.com:7888" }
            });
        }

        public string Protocol { get; set; }
        public string Scheme { get; set; }
        public string Method { get; set; }
        public string PathBase { get; set; }
        public string Path { get; set; }
        public string QueryString { get; set; }
        public string RawTarget { get; set; }
        public IHeaderDictionary Headers { get; set; }

        public Stream Body { get; set; }
    }
}
