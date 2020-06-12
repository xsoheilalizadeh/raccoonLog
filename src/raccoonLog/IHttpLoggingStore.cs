using System.Threading;
using System.Threading.Tasks;

namespace raccoonLog
{
    public interface IHttpLoggingStore
    {
        Task StoreAsync(LogContext logContext, CancellationToken cancellationToken = default);
    }

    public class DefaultHttpLoggingStore : IHttpLoggingStore
    {
        public Task StoreAsync(LogContext logContext, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}