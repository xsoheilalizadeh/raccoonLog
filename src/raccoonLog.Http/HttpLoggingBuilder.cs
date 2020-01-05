using Microsoft.Extensions.DependencyInjection;

namespace raccoonLog.Http
{
    public class HttpLoggingBuilder
    {
        public HttpLoggingBuilder(IServiceCollection services)
        {
            Services = services;
        }

        internal IServiceCollection Services { get; set; }
    }
}
