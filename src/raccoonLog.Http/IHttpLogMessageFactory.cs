using System.Threading;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public interface IHttpLogMessageFactory
    {
        Task<THttpMessageLog> Create<THttpMessageLog>(CancellationToken cancellationToken = default) where THttpMessageLog : HttpMessageLog, new();
    }
}
