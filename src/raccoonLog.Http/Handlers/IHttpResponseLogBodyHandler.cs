using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace raccoonLog.Http
{
    public interface IHttpResponseLogBodyHandler
    {
        Task Handle(Stream body, HttpResponseLog logMessage);

    }
}
