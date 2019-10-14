using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace raccoonLog.Http
{
    public class HttpLoggingProvider : IHttpLoggingProvider
    {
        private readonly IHttpLogResponseHandler _responseHandler;

        private readonly IHttpLogRequestHandler _requestHandler;

        private IOptions<RacconLogHttpOptions> _options;

        public HttpLoggingProvider(IHttpLogResponseHandler responseHandler,
            IHttpLogRequestHandler requestHandler,
            IOptions<RacconLogHttpOptions> options)
        {
            _responseHandler = responseHandler;
            _requestHandler = requestHandler;
            _options = options;
        }

        public Task Log(HttpRequest request)
        {
            if (request == null)
            {
                throw new NullReferenceException(nameof(request));
            }

            return LogRequest(request);
        }


        public Task Log(HttpResponse response, Stream bodyStream)
        {

            if (response == null)
            {
                throw new NullReferenceException(nameof(response));
            }

            return LogResponse(response, bodyStream);
        }

        private async Task LogResponse(HttpResponse response, Stream bodyStream)
        {
            var logMessage = await _responseHandler.Hendle(response, bodyStream);

            var options = _options.Value;

            Debug.WriteLine(JsonSerializer.Serialize(logMessage, options.JsonSerializerOptions));

            // store log message
        }

        private async Task LogRequest(HttpRequest request)
        {
            var logMessage = await _requestHandler.Hendle(request);

            var options = _options.Value;

            Debug.WriteLine(JsonSerializer.Serialize(logMessage, options.JsonSerializerOptions));

            // store log Message
        }

    }
}
