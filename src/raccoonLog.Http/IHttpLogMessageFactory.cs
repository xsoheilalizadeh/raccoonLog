using Microsoft.AspNetCore.Http;

namespace raccoonLog.Http
{
    public interface IHttpLogMessageFactory
    {
        HttpRequestLog Create(HttpRequest request);

        HttpResponseLog Create(HttpResponse request);
    }
}
