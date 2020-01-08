using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace raccoonLog.Http.Handlers
{
    public interface IHttpRequestLogBodyHandler
    {
        ValueTask Handle(Stream body, HttpRequestLog logMessage, CancellationToken cancellationToken = default);
    }
}