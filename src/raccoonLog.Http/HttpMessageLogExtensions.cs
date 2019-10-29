using System;

namespace raccoonLog.Http
{
    public static class HttpMessageLogExtensions
    {
        public static bool IsJson(this HttpMessageLog messageLog)
        {
            return IsJsonContentType(messageLog.ContentType);
        }

        private static bool IsJsonContentType(ReadOnlySpan<char> contentType)
        {
            return contentType.IndexOf("application/json", StringComparison.Ordinal) > -1;
        }
    }
}
