using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace raccoonLog.Http
{
    public interface IHttpMessageLogTraceIdHandler
    {
        Task Handle(HttpContext context, HttpMessageLog logMessage);
    }
}
