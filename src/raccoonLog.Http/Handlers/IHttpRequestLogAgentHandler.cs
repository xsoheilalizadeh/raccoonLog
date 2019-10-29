using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public interface IHttpRequestLogAgentHandler
    {
        Task Handle(HttpRequest request, HttpRequestLog logMessage);
    }
}
    