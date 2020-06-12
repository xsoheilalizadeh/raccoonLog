using Microsoft.Extensions.DependencyInjection;

namespace raccoonLog
{
    public class RaccoonLogBuilder
    {
        public RaccoonLogBuilder(IServiceCollection services)
        {
            Services = services;
        }

        internal IServiceCollection Services { get; set; }
    }
}