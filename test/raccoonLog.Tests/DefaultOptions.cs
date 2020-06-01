using Microsoft.Extensions.Options;
using raccoonLog.Http;

namespace raccoonLog.Tests
{
    public class DefaultOptions : IOptions<RaccoonLogHttpOptions>
    {
        public static IOptions<RaccoonLogHttpOptions> Default => new DefaultOptions();

        public RaccoonLogHttpOptions Value => new RaccoonLogHttpOptions();
    }
}
