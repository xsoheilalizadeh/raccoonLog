using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Http;
using raccoonLog.Mocking;

namespace raccoonLog.Benchmarks
{
    [MemoryDiagnoser]
    [ShortRunJob]
    public class MessageFactoryBenchmarks
    {
        private HttpContext _context;

        private HttpLogMessageFactory _factory;

        [GlobalSetup]
        public void SetUp()
        {
            _context = new DefaultHttpContext();
            _context.Features.Set(FakeHttpRequest.Value);
            _context.Features.Set(FakeHttpResponse.Value);
            _factory = new HttpLogMessageFactory(DefaultOptions.Default, NullProtector.Value);
        }

        [Benchmark]
        public HttpRequestLog CreateRequestLog()
        {
            return _factory.Create(_context.Request);
        }

        [Benchmark]
        public HttpResponseLog CreateResponseLog()
        {
            return _factory.Create(_context.Response);
        }

        [GlobalCleanup]
        public void CleanUp()
        {
            _context = null;
        }
    }
}