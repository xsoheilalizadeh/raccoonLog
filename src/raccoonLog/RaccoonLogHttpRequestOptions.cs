using Microsoft.Net.Http.Headers;

namespace raccoonLog
{
    public class RaccoonLogHttpRequestOptions : RaccoonLogHttpMessageOptions
    {
        public RaccoonLogHttpRequestOptions()
        {
            IgnoreHeaders.Add(HeaderNames.Cookie);
        }

        public HttpRequestLogSensitiveDataOptions SensitiveData { get; } = new HttpRequestLogSensitiveDataOptions();
    }
}