using System;

namespace raccoonLog.Http
{
    public class UrlLog
    {
        public UrlLog(Uri uri)
        {
            Host = uri.Host;
            Scheme = uri.Scheme;
            Path = uri.AbsolutePath;
            Port = uri.Port;
        }

        public int Port { get; private set; }

        public string Path { get; private set; }

        public string Host { get; private set; }

        public string Scheme { get; private set; }
    }
}

