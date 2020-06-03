using System;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

    public class ElasticSearchStore : IHttpLoggingStore
    {
        private readonly ElasticSearchStoreOptions _options;

        private readonly ElasticLowLevelClient _client;

        private readonly ILogger<ElasticSearchStore> _logger;

        public ElasticSearchStore(IOptions<ElasticSearchStoreOptions> options)
        {
            _options = options.Value;

            _client = new ElasticLowLevelClient(_options.Configuration);
        }

        public async Task StoreAsync(LogContext logContext, CancellationToken cancellationToken = default)
        {
            var client = new ElasticLowLevelClient(_options.Configuration);

            var response = await client.IndexAsync<BytesResponse>(_options.Index, "1", PostData.Serializable(logContext));

            if (!response.Success)
            {
                _logger.LogError(response.OriginalException, response.OriginalException.Message);
            }
        }
    }
}