namespace raccoonLog.Mocking
{
    public class NullProtector : IDataProtector
    {
        public static readonly IDataProtector Value = new NullProtector();

        public string Protect(string value)
        {
            return default;
        }

        public string Protect<TValue>(TValue value)
        {
            return default;
        }
    }
}