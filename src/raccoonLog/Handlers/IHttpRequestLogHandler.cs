using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace raccoonLog.Handlers
{
    public interface IHttpRequestLogHandler
    {
        ValueTask<HttpRequestLog> Handle(HttpRequest request, CancellationToken cancellationToken = default);
    }
}
