using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
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
            _context.Features.Set<IHttpRequestFeature>(FakeHttpRequest.Value);
            _context.Features.Set<IHttpResponseFeature>(FakeHttpResponse.Value);
            _factory = new HttpLogMessageFactory(DefaultOptions.Default, NullProtector.Value);
        }

        [Benchmark]
        public HttpRequestLog CreateRequestLog() => _factory.Create(_context.Request);
        
        [Benchmark]
        public HttpResponseLog CreateResponseLog() => _factory.Create(_context.Response);

        [GlobalCleanup]
        public void CleanUp()
        {
            _context = null;
        }
    }
}