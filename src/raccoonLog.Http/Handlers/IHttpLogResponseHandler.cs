using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public interface IHttpLogResponseHandler
    {
        Task<HttpResponseLog> Hendle(HttpResponse response, Stream bodyStream);
    }
}
        