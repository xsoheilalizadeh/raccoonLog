using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace raccoonLog.Http
{
    public interface IHttpLogMessageFactory
    {
        HttpRequestLog Create(HttpRequest request);

        HttpResponseLog Create(HttpResponse request);
    }
}
