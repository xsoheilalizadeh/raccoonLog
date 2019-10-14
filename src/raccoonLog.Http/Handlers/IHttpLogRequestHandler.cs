using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public interface IHttpLogRequestHandler
    {
        Task<HttpRequestLog> Hendle(HttpRequest request);
    }
}
