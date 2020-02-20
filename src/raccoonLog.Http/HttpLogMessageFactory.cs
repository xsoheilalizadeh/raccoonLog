using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using raccoonLog.Http.Handlers;

namespace raccoonLog.Http
{
    public class HttpLogMessageFactory : IHttpLogMessageFactory
    {
        private readonly IDataProtector _dataProtector;

        private readonly IOptions<RaccoonLogHttpOptions> _options;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IHttpMessageLogTraceIdHandler _traceIdHandler;

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

        public async ValueTask<THttpMessageLog> Create<THttpMessageLog>(CancellationToken cancellationToken)
            where THttpMessageLog : HttpMessageLog, new()
        {
            var context = _httpContextAccessor.HttpContext;

            var logMessage = new THttpMessageLog();

            await _traceIdHandler.Handle(context, logMessage);

            SetCommonLogProperties(logMessage, cancellationToken);

            return logMessage;
        }


        private void SetCommonLogProperties<THttpMessageLog>(THttpMessageLog logMessage,
            CancellationToken cancellationToken) where THttpMessageLog : HttpMessageLog, new()
        {
            var context = _httpContextAccessor.HttpContext;

            var user = context.User;

            var options = _options.Value;

            SetClaims(logMessage, user.Claims, options.SensitiveData.Claims, cancellationToken);

            if (logMessage is HttpRequestLog requestLog)
            {
                SetRequestLogProperties(requestLog, context, options, cancellationToken);
            }
            else
            {
                SetResponseLogProperties(logMessage, context, options, cancellationToken);
            }
        }


        private void SetResponseLogProperties<THttpMessageLog>(THttpMessageLog logMessage,
            HttpContext context,
            RaccoonLogHttpOptions options, CancellationToken cancellationToken)
            where THttpMessageLog : HttpMessageLog, new()
        {
            cancellationToken.ThrowIfCancellationRequested();

            var response = context.Response;

            var responseOptions = options.Response;

            var ignoreHeaders = responseOptions.IgnoreHeaders;

            var ignoreContentTypes = responseOptions.IgnoreContentTypes;

            var sensitiveData = options.SensitiveData.Response.Headers;

            logMessage.ContentType = response.ContentType;

            SetHeaders(logMessage, ignoreHeaders, sensitiveData, response.Headers, cancellationToken);

            if (ignoreContentTypes.Contains(response.ContentType))
            {
                logMessage.IgnoreBody();
            }
        }


        private void SetRequestLogProperties(HttpRequestLog logMessage,
            HttpContext context,
            RaccoonLogHttpOptions options, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var request = context.Request;

            var requestOptions = options.Request;

            var ignoreHeaders = requestOptions.IgnoreHeaders;

            var sensitiveData = options.SensitiveData.Request;

            logMessage.Method = request.Method;

            logMessage.ContentType = request.ContentType;

            SetCookies(logMessage, sensitiveData.Cookies, request.Cookies, cancellationToken);

            SetHeaders(logMessage, ignoreHeaders, sensitiveData.Headers, request.Headers, cancellationToken);

            SetParameters(logMessage, sensitiveData.Parameters, request.Query, cancellationToken);

            logMessage.SetUrl(request.GetEncodedUrl(), request.Protocol);

            if (requestOptions.IgnoreContentTypes.Contains(request.ContentType))
            {
                logMessage.IgnoreBody();
            }
        }


        private void SetHeaders<THttpMessageLog>(THttpMessageLog logMessage,
            IList<string> ignoreHeaders,
            Dictionary<string, ProtectType> sensitiveData,
            IHeaderDictionary headers, CancellationToken cancellationToken)
            where THttpMessageLog : HttpMessageLog, new()
        {
            foreach (var header in headers)
            {
                cancellationToken.ThrowIfCancellationRequested();

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

            RemoveSensitiveCookiesFromHeader(logMessage);
        }

        private void RemoveSensitiveCookiesFromHeader<THttpMessageLog>(THttpMessageLog logMessage)
            where THttpMessageLog : HttpMessageLog, new()
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
            Dictionary<string, ProtectType> senstiveData, CancellationToken cancellationToken)
            where THttpMessageLog : HttpMessageLog, new()
        {
            foreach (var claim in claims)
            {
                cancellationToken.ThrowIfCancellationRequested();

                string claimValue;

                if (senstiveData.TryGetValue(claim.Type, out var protectType))
                {
                    claimValue = _dataProtector.Protect(claim.Value, protectType);
                }
                else
                {
                    claimValue = claim.Value;
                }

                logMessage.Claims.TryAdd(claim.Type, claimValue);
            }
        }

        private void SetCookies(HttpRequestLog logMessage,
            Dictionary<string, ProtectType> sensitiveData,
            IRequestCookieCollection cookies, CancellationToken cancellationToken)
        {
            foreach (var cookie in cookies)
            {
                cancellationToken.ThrowIfCancellationRequested();

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
            IQueryCollection queries, CancellationToken cancellationToken)
        {
            foreach (var query in queries)
            {
                cancellationToken.ThrowIfCancellationRequested();

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