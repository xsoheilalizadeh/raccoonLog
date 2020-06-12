using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;

namespace raccoonLog
{
    public class HttpLogMessageFactory : IHttpLogMessageFactory
    {
        private readonly IDataProtector _dataProtector;

        private readonly RaccoonLogHttpOptions _options;

        public HttpLogMessageFactory(
            IOptions<RaccoonLogHttpOptions> options,
            IDataProtector dataProtector)
        {
            _dataProtector = dataProtector;
            _options = options.Value;
        }

        public HttpRequestLog Create(HttpRequest request)
        {
            var parameters = Map(request.Query, _dataProtector,
                _options.Request.SensitiveData.Parameters,
                null);

            var headers = Map(request.Headers, _dataProtector,
                _options.Request.SensitiveData.Headers,
                _options.Request.IgnoreHeaders);

            var cookies = Map(request.Cookies, _dataProtector,
                _options.Request.SensitiveData.Cookies, null);

            var uri = new Uri(request.GetDisplayUrl());

            var url = new UrlLog(uri.Port, uri.AbsolutePath, uri.Host, uri.Scheme, parameters);

            return new HttpRequestLog(url, request.Method, request.ContentType, headers, cookies);
        }


        public HttpResponseLog Create(HttpResponse response)
        {
            var headers = Map(response.Headers, _dataProtector,
                _options.Response.SensitiveData.Headers,
                _options.Response.IgnoreHeaders);

            return new HttpResponseLog(response.StatusCode, response.ContentType, headers);
        }

        public static List<KeyValuePair<string, string>> Map<TValue>(
            IEnumerable<KeyValuePair<string, TValue>> collection,
            IDataProtector protector, HashSet<string> protectedData, HashSet<string>? ignoredData)
        {
            var items = new List<KeyValuePair<string, string>>();

            foreach (var item in collection)
            {
                if (ignoredData is object && ignoredData.Contains(item.Key)) continue;

                var value = item.Value.ToString();

                items.Add(protectedData.Contains(item.Key)
                    ? new KeyValuePair<string, string>(item.Key, protector.Protect(value))
                    : new KeyValuePair<string, string>(item.Key, value));
            }

            return items;
        }
    }
}