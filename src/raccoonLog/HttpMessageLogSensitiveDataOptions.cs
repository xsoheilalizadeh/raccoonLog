using System.Collections.Generic;

namespace raccoonLog
{
    public class HttpMessageLogSensitiveDataOptions
    {
        public HashSet<string> Headers { get; set; } = new HashSet<string>();
    }
}