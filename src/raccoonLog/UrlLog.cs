using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace raccoonLog
{
    public class UrlLog
    {
        public UrlLog(int port, string path, string host, string scheme,
            IReadOnlyList<KeyValuePair<string, string>> parameters)
        {
            Port = port;
            Path = path;
            Host = host;
            Scheme = scheme;
            Parameters = parameters;
        }

        public static UrlLog Default =>
            new UrlLog(80, "/", "localhost", "http", new List<KeyValuePair<string, string>>());


        public int Port { get; }

        public string Path { get; }

        public string Host { get; }

        public string Scheme { get; }

        public string Absolute => ToString();

        public IReadOnlyList<KeyValuePair<string, string>> Parameters { get; }

        public override string ToString()
        {
            var query = QueryString.Create(Parameters).Value;

            return new UriBuilder(Scheme, Host, Port, Path, query).Uri.ToString();
        }
    }
}