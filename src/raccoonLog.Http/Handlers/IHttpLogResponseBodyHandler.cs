using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace raccoonLog.Http
{
    public interface IHttpLogResponseBodyHandler
    {
        Task Handle(HttpResponse response, HttpResponseLog logMessage, Stream bodyStream);
    }
}
