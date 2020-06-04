using System;
using Microsoft.Extensions.DependencyInjection;

namespace raccoonLog.Stores.ElasticSearch
{
    public static class ElasticSearchStoreExtensions
    {
        public static void AddElasticSearchStore(this HttpLoggingBuilder builder, Action<ElasticSearchStoreOptions> configureOptions)
        {
            var services = builder.Services;

            builder.AddStore<ElasticSearchStore>(ServiceLifetime.Singleton);

            services.Configure(configureOptions);
        }
    }
}