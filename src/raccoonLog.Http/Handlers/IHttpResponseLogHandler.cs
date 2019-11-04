using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace raccoonLog.Http.Handlers
{
    public interface IHttpResponseLogHandler
    {
        Task<HttpResponseLog> Handle(HttpResponse response, Stream body);
    }
}
            