using Microsoft.Extensions.DependencyInjection;
using System;

namespace raccoonLog
{
    public static class RaccoonLogServiceCollectionExtensions
    {
        public static void AddRaccoongLog(this IServiceCollection services, Action<RacconLogBuilder> builder)
        {
            var logBuilder = new RacconLogBuilder(services);

            builder(logBuilder);
        }
    }
}
    