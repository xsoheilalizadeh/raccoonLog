using System.Threading;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public interface IHttpLogMessageFactory
    {
        ValueTask<THttpMessageLog> Create<THttpMessageLog>(CancellationToken cancellationToken = default) where THttpMessageLog : HttpMessageLog, new();
    }
}
