using Microsoft.AspNetCore.Http;

namespace raccoonLog
{
    public interface IHttpLogMessageFactory
    {
        HttpRequestLog Create(HttpRequest request);

        HttpResponseLog Create(HttpResponse request);
    }
}