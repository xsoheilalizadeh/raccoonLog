using System.Threading;
using System.Threading.Tasks;

namespace raccoonLog.UnitTests.Functional
{
    public class InMemoryStore : IHttpLoggingStore
    {
        public static LogContext Context { get; set; }

        public Task StoreAsync(LogContext logContext, CancellationToken cancellationToken = default)
        {
            Context = logContext;

            return default;
        }
    }
}