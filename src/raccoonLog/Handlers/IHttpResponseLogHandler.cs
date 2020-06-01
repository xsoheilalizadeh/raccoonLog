using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace raccoonLog.Handlers
{
    public interface IHttpResponseLogHandler
    {
        ValueTask<HttpResponseLog> Handle(HttpResponse response, CancellationToken cancellationToken = default);
    }
}
