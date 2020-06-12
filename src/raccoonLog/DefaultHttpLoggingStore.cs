using System.Threading;
using System.Threading.Tasks;

namespace raccoonLog
{
    public class DefaultHttpLoggingStore : IHttpLoggingStore
    {
        public Task StoreAsync(LogContext logContext, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}