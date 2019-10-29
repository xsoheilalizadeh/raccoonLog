using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public interface IHttpLogMessageFactory
    {
        Task<THttpMessageLog> Create<THttpMessageLog>() where THttpMessageLog : HttpMessageLog, new();
    }
}
