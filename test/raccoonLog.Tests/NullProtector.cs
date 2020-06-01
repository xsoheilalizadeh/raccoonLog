using raccoonLog.Http;

namespace raccoonLog.Tests
{
    public class NullProtector : IDataProtector
    {
        public static IDataProtector Value = new NullProtector();

        public string Protect(string value)
        {
            return default;
        }
    }
}
