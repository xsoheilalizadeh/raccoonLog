using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public interface IHttpLoggingProvider
    {
        Task LogAsync(HttpRequest request, CancellationToken cancellationToken);

        Task LogAsync(HttpResponse response, Stream body, CancellationToken cancellationToken);
    }
}