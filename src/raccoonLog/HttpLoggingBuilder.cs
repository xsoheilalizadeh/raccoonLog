using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

[assembly:InternalsVisibleTo("raccoonLog.Stores.File")]
[assembly:InternalsVisibleTo("raccoonLog.Stores.ElasticSearch")]

namespace raccoonLog
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
