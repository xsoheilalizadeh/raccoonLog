using Elasticsearch.Net;

namespace raccoonLog.Stores.ElasticSearch
{
    public class ElasticSearchStoreOptions
    {
        public string Index { get; set; }

        public ConnectionConfiguration Configuration { get; set; }
    }
}