namespace raccoonLog
{
    public class DataProtector : IDataProtector
    {
        public string Protect(string value) => "protected-data";
    }
}