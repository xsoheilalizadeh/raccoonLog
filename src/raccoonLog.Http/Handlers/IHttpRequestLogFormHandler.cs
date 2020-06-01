using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace raccoonLog.Http.Handlers
{
    public interface IHttpRequestLogFormHandler
    {
        ValueTask Handle(HttpRequest request, HttpRequestLog logMessage, CancellationToken cancellationToken = default);
    }
}
