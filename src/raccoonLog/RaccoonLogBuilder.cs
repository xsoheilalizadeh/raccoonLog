using Microsoft.Extensions.DependencyInjection;

namespace raccoonLog
{
    public class RaccoonLogBuilder
    {
        public RaccoonLogBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; set; }


    }
}
