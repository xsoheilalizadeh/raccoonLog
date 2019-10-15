using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public interface IHttpRequestLogHandler
    {
        Task<HttpRequestLog> Handle(HttpRequest request);
    }
}
