using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace raccoonLog
{
    public interface IHttpLoggingProvider
    {
        ValueTask LogAsync(HttpContext context, CancellationToken cancellationToken = default);
    }
}
