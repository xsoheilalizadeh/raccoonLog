using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;
using UAParser;

namespace raccoonLog.Http
{
    public class DefaultHttpLogAgentHandler : IHttpLogAgentHandler
    {
        public Task<AgentLog> Handle(HttpRequest request)
        {
            if (request.Headers.TryGetValue(HeaderNames.UserAgent, out var agent))
            {
                var uaParser = Parser.GetDefault();

                var clientInfo = uaParser.Parse(agent);

                var ua = clientInfo.UA.ToString();

                var userAgent = ua == "Other" ? clientInfo.String : ua;

                var agentLog = new AgentLog(clientInfo.OS.ToString(), clientInfo.Device.ToString(), userAgent);

                return Task.FromResult(agentLog);
            }

            return Task.FromResult<AgentLog>(null);
        }
    }
}
