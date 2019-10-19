using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public interface IHttpLogMessageFactory
    {
        Task<THttpMessageLog> Create<THttpMessageLog>() where THttpMessageLog : HttpMessageLog, new();
    }
}
