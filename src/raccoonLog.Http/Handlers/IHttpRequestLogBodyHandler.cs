using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace raccoonLog.Http
{
    public interface IHttpRequestLogBodyHandler
    {
        Task Handle(Stream body, HttpRequestLog logMessage);
    }
}
