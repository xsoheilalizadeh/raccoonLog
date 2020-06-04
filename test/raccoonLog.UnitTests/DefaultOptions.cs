using Microsoft.Extensions.Options;

namespace raccoonLog.UnitTests
{
    public class DefaultOptions : IOptions<RaccoonLogHttpOptions>
    {
        public static IOptions<RaccoonLogHttpOptions> Default => new DefaultOptions();

        public RaccoonLogHttpOptions Value => new RaccoonLogHttpOptions();
    }
}
