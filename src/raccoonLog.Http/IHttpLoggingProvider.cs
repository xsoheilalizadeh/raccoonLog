using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public interface IHttpLoggingProvider
    {
        ValueTask LogAsync(HttpContext context, CancellationToken cancellationToken = default);
    }
}