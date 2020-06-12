using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace raccoonLog
{
    public interface IHttpLoggingProvider
    {
        ValueTask LogAsync(HttpContext context, CancellationToken cancellationToken = default);
    }
}