using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using UAParser;

namespace raccoonLog.Http.Handlers
{
    public class DefaultHttpRequestLogAgentHandler : IHttpRequestLogAgentHandler
    {
        public Task Handle(HttpRequest request, HttpRequestLog logMessage)
        {
            if (request == null)
            {
                throw new NullReferenceException(nameof(request));
            }

            if (logMessage == null)
            {
                throw new NullReferenceException(nameof(logMessage));
            }

            if (request.Headers.TryGetValue(HeaderNames.UserAgent, out var agent))
            {
                var uaParser = Parser.GetDefault();

                var clientInfo = uaParser.Parse(agent);

                var os = new OsLog(clientInfo.OS.Family, clientInfo.OS.Major);

                var userAgent = new UserAgentLog(clientInfo.UA.Family, clientInfo.UA.Major);

                logMessage.Agent = new AgentLog(os, userAgent, agent);
            }

            return Task.CompletedTask;
        }
    }
}
