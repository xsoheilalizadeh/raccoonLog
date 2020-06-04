using System.Text.Json;

namespace raccoonLog.Stores.ElasticSearch
{
    public class ElasticSearchStoreOptions
    {
        public string Index { get; set; }

        public string Url { get; set; }

        public JsonSerializerOptions SerializerOptions { get; set; }
    }
}