using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace raccoonLog.Http.Handlers
{
    public interface IHttpRequestLogAgentHandler
    {
        Task Handle(HttpRequest request, HttpRequestLog logMessage);
    }
}
    