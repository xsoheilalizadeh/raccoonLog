namespace raccoonLog
{
    public interface IDataProtector
    {
        string Protect(string value);
    }

    public class DataProtector : IDataProtector
    {
        public string Protect(string value) => "protected-data";
    }
}