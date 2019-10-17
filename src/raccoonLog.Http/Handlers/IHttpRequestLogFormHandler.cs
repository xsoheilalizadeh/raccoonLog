using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace raccoonLog.Http
{
    public interface IHttpRequestLogFormHandler
    {
        Task Handle(HttpRequest request, HttpRequestLog logMessage);
    }
}