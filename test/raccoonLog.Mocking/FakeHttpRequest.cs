using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace raccoonLog.Mocking
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

            var headers = new Dictionary<string, StringValues>
            {
                {"X-Custom", "noo"},
                {HeaderNames.Host, "ex.com:7888"}
            };

            _ = Enumerable.Range(1, 33).Select(index =>
            {
                headers.Add($"H-{index}", Guid.NewGuid().ToString("N"));
                return 1;
            }).ToList();

            Headers = new HeaderDictionary(headers);
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
        
        public static IHttpRequestFeature Value => new FakeHttpRequest();
    }
}