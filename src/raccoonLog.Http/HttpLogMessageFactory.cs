using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Primitives;

namespace raccoonLog.Http
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
            var url = new UrlLog(new Uri(request.GetDisplayUrl()));

            var sensitiveData = _options.Request.SensitiveData;

            var parameters = request.Query
                    .Select(ApplyProtection(sensitiveData.Parameters))
                    .ToList();

            var headers = request.Headers
                    .Where(q => !_options.Request.IgnoreHeaders.Any(q.Key.Equals))
                    .Select(ApplyProtection(sensitiveData.Headers))
                    .ToList();

            var cookies = request.Cookies.Select(item =>
            {
                if (sensitiveData.Cookies.Contains(item.Key))
                {
                    return new KeyValuePair<string, string>(item.Key, _dataProtector.Protect(item.Value));
                }

                return new KeyValuePair<string, string>(item.Key, item.Value );
            }).ToList();

            return new HttpRequestLog(url, request.Method, parameters, headers, cookies, request.ContentType);
        }


        public HttpResponseLog Create(HttpResponse request)
        {
            var sensitiveData = _options.Response.SensitiveData;

            var headers = request.Headers
               .Where(q => !_options.Response.IgnoreHeaders.Any(q.Key.Equals))
               .Select(ApplyProtection(sensitiveData.Headers))
               .ToList();

            return new HttpResponseLog(request.StatusCode, request.ContentType, headers);
        }

        private Func<KeyValuePair<string, StringValues>, KeyValuePair<string, StringValues>> ApplyProtection(List<string> sensitiveData)
        {
            return item =>
            {
                if (sensitiveData.Contains(item.Key))
                {
                    return new KeyValuePair<string, StringValues>(item.Key, _dataProtector.Protect(item.Value));
                }
                return new KeyValuePair<string, StringValues>(item.Key, item.Value);
            };
        }
    }
}