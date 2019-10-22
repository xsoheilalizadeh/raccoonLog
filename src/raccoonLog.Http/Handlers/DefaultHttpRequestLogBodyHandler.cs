using System;
using System.IO;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace raccoonLog.Http
{
    public class DefaultHttpRequestLogBodyHandler : BaseHttpMessageLogBodyHandler<HttpRequestLog>, IHttpRequestLogBodyHandler
    {
        public DefaultHttpRequestLogBodyHandler(IOptions<RaccoonLogHttpOptions> options) : base(options)
        {
        }
    }
}

