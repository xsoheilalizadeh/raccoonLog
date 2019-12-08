using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace raccoonLog.Http.Handlers
{
    public interface IHttpRequestLogHandler
    {
        Task<HttpRequestLog> Handle(HttpRequest request, CancellationToken cancellationToken = default);
    }
}
