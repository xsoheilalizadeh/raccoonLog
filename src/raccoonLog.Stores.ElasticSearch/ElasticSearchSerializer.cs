using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Microsoft.Extensions.Options;

namespace raccoonLog.Stores.ElasticSearch
{
    public class ElasticSearchSerializer : IElasticsearchSerializer
    {
        private readonly ElasticSearchStoreOptions _options;

        public ElasticSearchSerializer(IOptions<ElasticSearchStoreOptions> options)
        {
            _options = options.Value;
        }

        public object Deserialize(Type type, Stream stream)
        {
            var buffer = new Span<byte>();

            stream.Read(buffer);

            return JsonSerializer.Deserialize(buffer, type, _options.SerializerOptions);
        }

        public T Deserialize<T>(Stream stream)
        {
            var buffer = new Span<byte>();

            stream.Read(buffer);

            return JsonSerializer.Deserialize<T>(buffer, _options.SerializerOptions);
        }

        public Task<object> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = default)
        {
            return JsonSerializer.DeserializeAsync(stream, type, _options.SerializerOptions, cancellationToken)
                .AsTask();
        }

        public Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default)
        {
            return JsonSerializer.DeserializeAsync<T>(stream, _options.SerializerOptions, cancellationToken).AsTask();
        }

        public void Serialize<T>(T data, Stream stream,
            SerializationFormatting formatting = SerializationFormatting.None)
        {
            var json = JsonSerializer.Serialize(data, _options.SerializerOptions);

            stream.Write(Encoding.UTF8.GetBytes(json));
        }

        public Task SerializeAsync<T>(T data, Stream stream,
            SerializationFormatting formatting = SerializationFormatting.None,
            CancellationToken cancellationToken = default)
        {
            return JsonSerializer.SerializeAsync(stream, data, _options.SerializerOptions, cancellationToken);
        }
    }
}