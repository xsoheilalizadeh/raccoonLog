using Microsoft.Extensions.DependencyInjection;
using System;

namespace raccoonLog
{
    public static class RaccoonLogServiceCollectionExtensions
    {
        public static void AddRaccoonLog(this IServiceCollection services, Action<RaccoonLogBuilder> builder)
        {
            var logBuilder = new RaccoonLogBuilder(services);

            builder(logBuilder);
        }
    }
}
    