using System.IO;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public interface IHttpRequestLogBodyHandler
    {
        Task Handle(Stream body, HttpRequestLog logMessage);
    }
}
