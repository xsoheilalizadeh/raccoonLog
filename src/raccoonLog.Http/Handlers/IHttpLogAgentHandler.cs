using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public interface IHttpLogAgentHandler
    {
        Task<AgentLog> Handle(HttpRequest request);
    }
}
