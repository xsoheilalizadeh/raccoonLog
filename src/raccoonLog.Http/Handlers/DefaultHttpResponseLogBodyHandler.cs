using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace raccoonLog.Http
{
    public class DefaultHttpResponseLogBodyHandler : BaseHttpMessageLogBodyHandler<HttpResponseLog>, IHttpResponseLogBodyHandler
    {
        public DefaultHttpResponseLogBodyHandler(IOptions<RaccoonLogHttpOptions> options) : base(options)
        {
        }
    }
}
