using System;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace raccoonLog.Stores.ElasticSearch
{
    public class ElasticSearchStore : IHttpLoggingStore
    {
        private readonly ElasticLowLevelClient _client;

        private readonly ILogger<ElasticSearchStore> _logger;
        private readonly ElasticSearchStoreOptions _options;

        public ElasticSearchStore(IOptions<ElasticSearchStoreOptions> options, ILogger<ElasticSearchStore> logger,
            IElasticsearchSerializer serializer)
        {
            _options = options.Value;
            _logger = logger;

            var pool = new SingleNodeConnectionPool(new Uri(_options.Url));
            var connection = new HttpConnection();
            var connectionConfiguration = new ConnectionConfiguration(pool, connection, serializer);

            _client = new ElasticLowLevelClient(connectionConfiguration);
        }

        public async Task StoreAsync(LogContext logContext, CancellationToken cancellationToken = default)
        {
            var response = await _client.IndexAsync<BytesResponse>(_options.Index, logContext.TraceId,
                PostData.Serializable(new ElasticLogContext(logContext)), ctx: cancellationToken);

            if (!response.Success) _logger.LogError(response.OriginalException, response.OriginalException.Message);
        }
    }
}