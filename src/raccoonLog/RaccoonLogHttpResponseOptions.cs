namespace raccoonLog
{
    public class RaccoonLogHttpResponseOptions : RaccoonLogHttpMessageOptions
    {
        public RaccoonLogHttpResponseOptions()
        {
            IgnoreContentTypes.Add("text/html; charset=utf-8");
        }

        public HttpResponseLogSensitiveDataOptions SensitiveData { get; } = new HttpResponseLogSensitiveDataOptions();
    }
}