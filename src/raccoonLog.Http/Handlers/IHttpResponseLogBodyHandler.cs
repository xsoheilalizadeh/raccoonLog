using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace raccoonLog.Http.Handlers
{
    public interface IHttpResponseLogBodyHandler
    {
        Task Handle(Stream body, HttpResponseLog logMessage, CancellationToken cancellationToken);

    }
}
