using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;

namespace raccoonLog.Stores.ElasticSearch
{

    public class ElasticSearchSerializer : IElasticsearchSerializer
    {
        public object Deserialize(Type type, Stream stream)
        {
            var buffer = new Span<byte>();

            stream.Read(buffer);

            return JsonSerializer.Deserialize(buffer, type);
        }

        public T Deserialize<T>(Stream stream)
        {
            var buffer = new Span<byte>();

            stream.Read(buffer);

            return JsonSerializer.Deserialize<T>(buffer);
        }

        public Task<object> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = default)
        {
            return JsonSerializer.DeserializeAsync(stream, type, cancellationToken: cancellationToken).AsTask();
        }

        public Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default)
        {
            return JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken).AsTask();
        }

        public void Serialize<T>(T data, Stream stream, SerializationFormatting formatting = SerializationFormatting.None)
        {
            var json = JsonSerializer.Serialize<T>(data);

            stream.Write(Encoding.UTF8.GetBytes(json));
        }

        public Task SerializeAsync<T>(T data, Stream stream, SerializationFormatting formatting = SerializationFormatting.None, CancellationToken cancellationToken = default)
        {
            return JsonSerializer.SerializeAsync<T>(stream, data, cancellationToken: cancellationToken);
        }
    }
}