using System;

namespace raccoonLog.Http
{
    public static class HttpMessageLogExtensions
    {
        private static bool IsJsonContentType(ReadOnlySpan<char> contentType)
        {
            return contentType.IndexOf("application/json", StringComparison.Ordinal) > -1;
        }
    }
}
