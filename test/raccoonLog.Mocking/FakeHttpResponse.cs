﻿#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;

namespace raccoonLog.Mocking
{
    public class FakeHttpResponse : IHttpResponseFeature
    {
        public FakeHttpResponse(string? body = null)
        {
            StatusCode = StatusCodes.Status200OK;

            var headers = new Dictionary<string, StringValues>
            {
                {"X-Custom", "boo"},
                {"X-Custom-Test", "boo"}
            };

            _ = Enumerable.Range(1, 33).Select(index =>
            {
                headers.Add($"H-{index}", Guid.NewGuid().ToString("N"));
                return 1;
            }).ToList();

            Headers = new HeaderDictionary(headers);


            var stream = new MemoryStream();

            if (body is object) stream.Write(Encoding.UTF8.GetBytes(body));

            Body = stream;
        }

        public static IHttpResponseFeature Value => new FakeHttpResponse();

        public int StatusCode { get; set; }

        public string ReasonPhrase { get; set; }

        public IHeaderDictionary Headers { get; set; }

        public Stream Body { get; set; }

        public bool HasStarted => true;

        public void OnCompleted(Func<object, Task> callback, object state)
        {
        }

        public void OnStarting(Func<object, Task> callback, object state)
        {
        }
    }
}