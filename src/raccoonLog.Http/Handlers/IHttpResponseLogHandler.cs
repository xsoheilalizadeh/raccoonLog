using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public interface IHttpResponseLogHandler
    {
        Task<HttpResponseLog> Handle(HttpResponse response, Stream body);
    }
}
            