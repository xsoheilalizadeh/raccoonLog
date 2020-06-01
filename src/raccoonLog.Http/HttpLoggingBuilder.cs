using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

[assembly:InternalsVisibleTo("raccoonLog.Stores.File")]
namespace raccoonLog.Http
{
    public class HttpLoggingBuilder
    {
        public HttpLoggingBuilder(IServiceCollection services)
        {
            Services = services;
        }

        protected internal IServiceCollection Services { get; set; }
    }
}
