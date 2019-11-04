using System.IO;
using System.Threading.Tasks;

namespace raccoonLog.Http.Handlers
{
    public interface IHttpRequestLogBodyHandler
    {
        Task Handle(Stream body, HttpRequestLog logMessage);
    }
}
