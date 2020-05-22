using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public interface IHttpLoggingProvider
    {
        ValueTask LogAsync(HttpRequest request, CancellationToken cancellationToken = default);

        ValueTask LogAsync(HttpResponse response, Stream body, CancellationToken cancellationToken = default);
    }
}