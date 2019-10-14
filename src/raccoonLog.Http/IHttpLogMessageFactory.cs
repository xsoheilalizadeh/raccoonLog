using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace raccoonLog.Http
{
    public interface IHttpLogMessageFactory
    {
        THttpMessageLog Create<THttpMessageLog>() where THttpMessageLog : HttpMessageLog, new();
    }
}
