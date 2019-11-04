using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using raccoonLog.Http.Handlers;

namespace raccoonLog.Http
{
    public class HttpLogMessageFactory : IHttpLogMessageFactory
    {
        private IHttpContextAccessor _httpContextAccessor;

        private IOptions<RaccoonLogHttpOptions> _options;

        private IHttpMessageLogTraceIdHandler _traceIdHandler;

        private IDataProtector _dataProtector;

        public HttpLogMessageFactory(IHttpContextAccessor httpContextAccessor,
            IHttpMessageLogTraceIdHandler traceIdHandler,
            IOptions<RaccoonLogHttpOptions> options,
            IDataProtector dataProtector)
        {
            _httpContextAccessor = httpContextAccessor;
            _traceIdHandler = traceIdHandler;
            _dataProtector = dataProtector;
            _options = options;
        }

        public async Task<THttpMessageLog> Create<THttpMessageLog>() where THttpMessageLog : HttpMessageLog, new()
        {
            var context = _httpContextAccessor.HttpContext;

            var logMessage = new THttpMessageLog();

            await _traceIdHandler.Handle(context, logMessage);

            SetCommonLogProperties(logMessage);

            return logMessage;
        }


        private void SetCommonLogProperties<THttpMessageLog>(THttpMessageLog logMessage) where THttpMessageLog : HttpMessageLog, new()
        {
            var context = _httpContextAccessor.HttpContext;

            var user = context.User;

            var options = _options.Value;

            SetClaims(logMessage, user.Claims, options.SensitiveData.Claims);

            if (logMessage is HttpRequestLog requestLog)
            {
                SetRequestLogProperties(requestLog, context, options);
            }
            else
            {
                SetResponseLogProperties(logMessage, context, options);
            }
        }


        private void SetResponseLogProperties<THttpMessageLog>(THttpMessageLog logMessage,
            HttpContext context,
            RaccoonLogHttpOptions options)
            where THttpMessageLog : HttpMessageLog, new()
        {
            var response = context.Response;

            var responseOptions = options.Response;

            var ignoreHeaders = responseOptions.IgnoreHeaders;

            var ignoreContentTypes = responseOptions.IgnoreContentTypes;

            var sestitiveData = options.SensitiveData.Response.Headers;

            logMessage.ContentType = response.ContentType;

            SetHeaders(logMessage, ignoreHeaders, sestitiveData, response.Headers);

            if (ignoreContentTypes.Contains(response.ContentType))
            {
                logMessage.IgnoreBody();
            }
        }


        private void SetRequestLogProperties(HttpRequestLog logMessage,
            HttpContext context,
            RaccoonLogHttpOptions options)
        {
            var request = context.Request;

            var requestOptions = options.Request;

            var ignoreHeaders = requestOptions.IgnoreHeaders;

            var sestitiveData = options.SensitiveData.Request;

            logMessage.Method = request.Method;

            logMessage.ContentType = request.ContentType;

            SetCookies(logMessage, sestitiveData.Cookies, request.Cookies);

            SetHeaders(logMessage, ignoreHeaders, sestitiveData.Headers, request.Headers);

            SetParameters(logMessage, sestitiveData.Parameters, request.Query);

            logMessage.SetUrl(request.GetEncodedUrl(), request.Protocol);

            if (requestOptions.IgnoreContentTypes.Contains(request.ContentType))
            {
                logMessage.IgnoreBody();
            }
        }


        private void SetHeaders<THttpMessageLog>(THttpMessageLog logMessage,
         IList<string> ignoreHeaders,
         Dictionary<string, ProtectType> sensitiveData,
         IHeaderDictionary headers)
         where THttpMessageLog : HttpMessageLog, new()
        {
            foreach (var header in headers)
            {
                if (!ignoreHeaders.Contains(header.Key))
                {
                    string headerValue;

                    if (sensitiveData.TryGetValue(header.Key, out var protectType))
                    {
                        headerValue = _dataProtector.Protect(header.Value, protectType);
                    }
                    else
                    {
                        headerValue = header.Value;
                    }

                    logMessage.Headers.Add(header.Key, headerValue);
                }
            }

            RemoveSenstiiveCookiesFromHeader(logMessage);
        }

        private void RemoveSenstiiveCookiesFromHeader<THttpMessageLog>(THttpMessageLog logMessage) where THttpMessageLog : HttpMessageLog, new()
        {
            if (logMessage is HttpResponseLog)
            {
                var option = _options.Value;    

                var sensitiveCookies = option.SensitiveData.Request.Cookies;

                if (logMessage.Headers.TryGetValue(HeaderNames.SetCookie, out var cookieHeader))
                {
                    var cookieSpan = cookieHeader.AsSpan();

                    var simicolonIndex = cookieHeader.IndexOf(';');

                    var equalIndex = cookieSpan.IndexOf('=');

                    var cookieName = cookieSpan.Slice(0, equalIndex);

                    if (sensitiveCookies.ContainsKey(cookieName.ToString()))
                    {
                        var cookieValue = cookieSpan.Slice(equalIndex + 1, (simicolonIndex - equalIndex) - 1);

                        var newCookieValue = cookieHeader.Replace(cookieValue.ToString(), "secret-value");

                        logMessage.Headers[HeaderNames.SetCookie] = newCookieValue;
                    }
                }
            }
        }

        private void SetClaims<THttpMessageLog>(THttpMessageLog logMessage,
         IEnumerable<Claim> claims,
         Dictionary<string, ProtectType> senstiveData)
         where THttpMessageLog : HttpMessageLog, new()
        {
            foreach (var claim in claims)
            {
                string claimValue;

                if (senstiveData.TryGetValue(claim.Type, out var protectType))
                {
                    claimValue = _dataProtector.Protect(claim.Value, protectType);
                }
                else
                {
                    claimValue = claim.Value;
                }

                logMessage.Claims.Add(claim.Type, claimValue);
            }
        }

        private void SetCookies(HttpRequestLog logMessage,
        Dictionary<string, ProtectType> sensitiveData,
        IRequestCookieCollection cookies)
        {
            foreach (var cookie in cookies)
            {
                string cookieValue;

                if (sensitiveData.TryGetValue(cookie.Key, out var protectType))
                {
                    cookieValue = _dataProtector.Protect(cookie.Value, protectType);
                }
                else
                {
                    cookieValue = cookie.Value;
                }

                logMessage.Cookies.Add(cookie.Key, cookieValue);
            }
        }


        private void SetParameters(HttpRequestLog logMessage,
            Dictionary<string, ProtectType> sensitiveData,
            IQueryCollection queries)
        {
            foreach (var query in queries)
            {
                string queryValue;

                if (sensitiveData.TryGetValue(query.Key, out var protectType))
                {
                    queryValue = _dataProtector.Protect(query.Value, protectType);
                }
                else
                {
                    queryValue = query.Value;
                }

                logMessage.Parameters.Add(query.Key, queryValue);
            }
        }

    }
}
