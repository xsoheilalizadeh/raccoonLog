using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public interface IHttpRequestLogAgentHandler
    {
        Task Handle(HttpRequest request, HttpRequestLog logMessage);
    }
}
    