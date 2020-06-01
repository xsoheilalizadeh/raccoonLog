using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using raccoonLog.Handlers;
using Xunit;

namespace raccoonLog.Tests
{

    public class HttpMessageLogBodyReaderTests
    {
        [Fact]
        public async Task ReadsJsonWhenContentTypeIsJson()
        {
            var bodyStream = new MemoryStream();
            var json = "{\"name\":\"my_name\"}";
            var reader = new HttpMessageLogBodyReader(new List<string>());

            bodyStream.Write(Encoding.UTF8.GetBytes(json));

            var body = await reader.ReadAsync(bodyStream, MediaTypeNames.Application.Json);

            Assert.Equal(body.ToString(), json);
        }

        [Fact]
        public async Task ReadsNullWhenContentLengthIsZero()
        {
            var bodyStream = new MemoryStream();

            var reader = new HttpMessageLogBodyReader(new List<string>());

            var body = await reader.ReadAsync(bodyStream, MediaTypeNames.Application.Json);

            Assert.Null(body);
        }

        [Fact]
        public async Task ReadsNullWhenContentIsIgnored()
        {
            var bodyStream = new MemoryStream();

            var ignoredContents = new List<string>() { MediaTypeNames.Text.Plain };

            var reader = new HttpMessageLogBodyReader(ignoredContents);

            var body = await reader.ReadAsync(bodyStream, MediaTypeNames.Text.Plain);

            Assert.Null(body);
        }
    }
}
