using System;
using System.Threading;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public interface IHttpLoggingStore
    {
        ValueTask StoreAsync(LogContext logContext, CancellationToken cancellationToken = default);

    }

    public class DefaultHttpLoggingStore : IHttpLoggingStore
    {
        public ValueTask StoreAsync(LogContext logContext, CancellationToken cancellationToken = default) => default;
    }
}
