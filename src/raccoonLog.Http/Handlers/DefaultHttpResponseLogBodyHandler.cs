using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace raccoonLog.Http
{
    public class DefaultHttpResponseLogBodyHandler : BaseHttpMessageLogBodyHandler<HttpResponseLog>, IHttpResponseLogBodyHandler
    {
    }
}
