namespace raccoonLog.UnitTests
{
    public class NullProtector : IDataProtector
    {
        public static readonly IDataProtector Value = new NullProtector();

        public string Protect(string value)
        {
            return default;
        }
    }
}
