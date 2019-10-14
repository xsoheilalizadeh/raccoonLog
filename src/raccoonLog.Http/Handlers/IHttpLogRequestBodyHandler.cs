using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace raccoonLog.Http
{
    public interface IHttpLogRequestBodyHandler
    {
        Task Handle(HttpRequest request, HttpRequestLog logMessage);
    }
}
