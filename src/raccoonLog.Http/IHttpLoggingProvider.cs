using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public interface IHttpLoggingProvider
    {
        Task Log(HttpRequest request);

        Task Log(HttpResponse response,Stream bodyStream);
    }
}   