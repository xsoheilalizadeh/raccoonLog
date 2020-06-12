using System;
using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;

namespace raccoonLog.Stores.ElasticSearch
{
    public static class ElasticSearchStoreExtensions
    {
        public static void AddElasticSearchStore(this HttpLoggingBuilder builder,
            Action<ElasticSearchStoreOptions> configureOptions)
        {
            var services = builder.Services;

            services.AddSingleton<IElasticsearchSerializer, ElasticSearchSerializer>();

            builder.AddStore<ElasticSearchStore>(ServiceLifetime.Singleton);

            services.Configure(configureOptions);
        }
    }
}