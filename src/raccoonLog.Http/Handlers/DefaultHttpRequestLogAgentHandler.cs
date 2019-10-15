using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;
using UAParser;

namespace raccoonLog.Http
{
    public class DefaultHttpRequestLogAgentHandler : IHttpRequestLogAgentHandler
    {
        public Task<AgentLog> Handle(HttpRequest request)
        {
            if (request == null)
            {
                throw new NullReferenceException(nameof(request));
            }

            if (request.Headers.TryGetValue(HeaderNames.UserAgent, out var agent))
            {
                var uaParser = Parser.GetDefault();

                var clientInfo = uaParser.Parse(agent);

                var os = new OsLog(clientInfo.OS.Family, clientInfo.OS.Major);

                var userAgent = new UserAgentLog(clientInfo.UA.Family, clientInfo.UA.Major);

                var agentLog = new AgentLog(os, userAgent, agent);

                return Task.FromResult(agentLog);
            }

            return Task.FromResult<AgentLog>(null);
        }
    }
}
