using System.Threading;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public interface IHttpLoggingStore
    {
        ValueTask StoreAsync(HttpRequestLog requestLog, CancellationToken cancellationToken = default);

        ValueTask StoreAsync(HttpResponseLog responseLog, CancellationToken cancellationToken = default);
    }

    public class DefaultHttpLoggingStore : IHttpLoggingStore
    {
        public ValueTask StoreAsync(HttpRequestLog requestLog, CancellationToken cancellationToken = default) => default;

        public ValueTask StoreAsync(HttpResponseLog responseLog, CancellationToken cancellationToken = default) => default;
    }
}
