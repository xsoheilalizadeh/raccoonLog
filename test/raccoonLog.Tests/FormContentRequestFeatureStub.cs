using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace raccoonLog.Tests.Handlers
{

    public class RequestCookiesFeatureStub : IRequestCookiesFeature
    {
        public RequestCookiesFeatureStub()
        {
            Cookies = new RequestCookieCollection(new Dictionary<string, string>
            {
                {"x_x","programmingisbullshit" }
            });
        }

        public IRequestCookieCollection Cookies { get; set; }
    }

    public class RequestFeatureStub : IHttpRequestFeature
    {
        public RequestFeatureStub()
        {
            Protocol = "HTTP/2";
            Scheme = "http";
            Path = "/this-is-path";
            QueryString = "?name=soheil&age=12";
            Body = new MemoryStream();
            Headers = new HeaderDictionary(new Dictionary<string, StringValues>
            {
                {"X-Custom","noo" },
                {HeaderNames.ContentType,"application/json" },
                {HeaderNames.Host,"ex.com" }
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

    public class IResponseFeatureStub : IHttpResponseFeature
    {

        public IResponseFeatureStub()
        {
            StatusCode = 200;
            Headers = new HeaderDictionary(new Dictionary<string, StringValues>
            {
                {"X-Custom","foo" },
                {HeaderNames.ContentType,"application/json" }
            });
        }

        public int StatusCode { get; set; }

        public string ReasonPhrase { get; set; }

        public IHeaderDictionary Headers { get; set; }

        public Stream Body { get; set; }

        public bool HasStarted => false;

        public void OnCompleted(Func<object, Task> callback, object state)
        {
        }

        public void OnStarting(Func<object, Task> callback, object state)
        {
        }
    }

    public class FormContentRequestFeatureStub : IFormFeature, IHttpRequestFeature
    {
        public FormContentRequestFeatureStub()
        {
            Form = new FormCollection(new Dictionary<string, StringValues>
                    {
                        {"Name", "Soheil"},
                        {"Age", "20"}
                    },
                new FormFileCollection
                {
                        new FormFile(Stream.Null, 0, 100, "image", "photo.png")
                        {
                            Headers = new HeaderDictionary
                            {
                                {HeaderNames.ContentType, MediaTypeNames.Text.Plain},
                                {HeaderNames.ContentDisposition, $"attachment; filename=\"{Guid.NewGuid():N}.text\""}
                            }
                        },
                        new FormFile(Stream.Null, 0, 100, "video", "video.mp4")
                        {
                            Headers = new HeaderDictionary
                            {
                                {HeaderNames.ContentType, MediaTypeNames.Text.Plain},
                                {HeaderNames.ContentDisposition, $"attachment; filename=\"{Guid.NewGuid():N}.text\""}
                            }
                        },
                });
            Protocol = "HTTP/2";
            Scheme = "http";
            Method = "GET";
            Path = "/some-path";
            Headers = new HeaderDictionary(new Dictionary<string, StringValues>
            {
                {HeaderNames.ContentType,"application/x-www-from-urlencoded" },
                {"X-Custom","boo" }
            });
            Body = new MemoryStream();
        }

        public bool HasFormContentType => true;

        public IFormCollection Form { get; set; }
        public string Protocol { get; set; }
        public string Scheme { get; set; }
        public string Method { get; set; }
        public string PathBase { get; set; }
        public string Path { get; set; }
        public string QueryString { get; set; }
        public string RawTarget { get; set; }
        public IHeaderDictionary Headers { get; set; }
        public Stream Body { get; set; }

        public IFormCollection ReadForm() => Form;

        public Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken) => Task.FromResult(Form);
    }
}