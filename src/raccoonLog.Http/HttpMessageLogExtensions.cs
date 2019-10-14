using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

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
