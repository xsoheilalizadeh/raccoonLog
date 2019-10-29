using System.IO;
using System.Threading.Tasks;

namespace raccoonLog.Http
{
    public interface IHttpResponseLogBodyHandler
    {
        Task Handle(Stream body, HttpResponseLog logMessage);

    }
}
