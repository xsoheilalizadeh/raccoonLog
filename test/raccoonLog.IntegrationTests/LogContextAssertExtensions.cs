using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace raccoonLog.IntegrationTests
{
    public static class LogContextAssertExtensions
    {
        public static void ShouldBe(this HttpRequestLog requestLog, HttpRequestMessage request)
        {
            foreach (var header in request.Headers)
            {
                var (key, value) = requestLog.Headers.First(h => h.Key == header.Key);

                Assert.Equal(header.Key, key);
                Assert.Equal(string.Join(',', header.Value), value);
            }

            if (request.Content is object)
            {
                var requestBodyStream = request.Content.ReadAsStreamAsync().GetAwaiter().GetResult();

                requestBodyStream.Position = 0;

                var requestBody = new StreamReader(requestBodyStream).ReadToEnd();

                Assert.Equal(requestBody, requestLog.Body?.ToString());
            }

            Assert.Equal(request.Method.Method, requestLog.Method);
            Assert.Equal(request.RequestUri, new Uri(requestLog.Url.ToString()));
        }

        public static void ShouldBe(this HttpResponseLog responseLog, HttpResponseMessage response)
        {
            foreach (var header in response.Headers)
            {
                var (key, value) = responseLog.Headers.First(h => h.Key == header.Key);

                Assert.Equal(header.Key, key);
                Assert.Equal(string.Join(',', header.Value), value);
            }

            var responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            Assert.Equal(responseBody, responseLog.Body?.ToString());
            Assert.Equal((int) response.StatusCode, responseLog.StatusCode);
        }
    }
}