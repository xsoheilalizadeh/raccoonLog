using System;

namespace raccoonLog.Http
{
    public class UrlLog
    {
        public UrlLog(Uri uri, string protocol)
        {
            Host = uri.Host;
            Protocol = protocol;
            Scheme = uri.Scheme;
            Path = uri.AbsolutePath;
        }

        public string Path { get; set; }

        public string Host { get; set; }

        public string Scheme { get; set; }

        public string Protocol { get; set; }
    }
}

