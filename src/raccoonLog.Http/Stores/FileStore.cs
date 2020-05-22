using System;
using Microsoft.Extensions.Options;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace raccoonLog.Http.Stores
{
    public class FileStore : IHttpLoggingStore, IDisposable
    {
#if NETCOREAPP2_2
        private readonly IHostingEnvironment _environment;
#elif NETCOREAPP3_0 || NETCOREAPP3_1
        private readonly IHostEnvironment _environment;
#endif

        private static bool HasLogDirectory;

        private FileStream _logFileStream;

        private IFileSystem _fileSystem;

        private readonly IOptions<FileStoreOptions> _options;
        private readonly IOptions<RaccoonLogHttpOptions> _logHttpOptions;

        private static readonly byte[] UTF8EndBracket = { (byte)']' };
        private static readonly byte[] UTF8StartBracket = { (byte)'[' };
        private static readonly byte[] UTF8Enter = { (byte)',', (byte)'\r', (byte)'\n' };

        private bool responseStored = false;

        internal FileStore(
            IOptions<RaccoonLogHttpOptions> logHttpOptions,
            IOptions<FileStoreOptions> options,
            IFileSystem fileSystem
        )
        {
            _options = options;
            _fileSystem = fileSystem;
            _logHttpOptions = logHttpOptions;
        }


#if NETCOREAPP2_2
        public FileStore(IHostingEnvironment environment,
            IOptions<RaccoonLogHttpOptions> logHttpOptions,
            IOptions<FileStoreOptions> options,
            IFileSystem fileSystem
        ) : this(logHttpOptions, options, fileSystem)
        {
            _environment = environment;
        }

#elif NETCOREAPP3_0 || NETCOREAPP3_1
        public FileStore(IHostEnvironment environment,
            IOptions<RaccoonLogHttpOptions> logHttpOptions,
            IOptions<FileStoreOptions> options,
            IFileSystem fileSystem
        ) : this(logHttpOptions, options,fileSystem)
        {
            _environment = environment;
        }
#endif

        public ValueTask StoreAsync(HttpRequestLog requestLog, CancellationToken cancellationToken = default)
        {
            return SaveLogMessageAsync(requestLog, cancellationToken);
        }

        public ValueTask StoreAsync(HttpResponseLog responseLog, CancellationToken cancellationToken = default)
        {
            return SaveLogMessageAsync(responseLog, cancellationToken);
        }

        private async ValueTask SaveLogMessageAsync(HttpMessageLog messageLog, CancellationToken cancellationToken = default)
        {
            var path = GetPath();

            var options = _logHttpOptions.Value;

            TryCreateLogDirectory(path);

            var filePath = GetSavePath(messageLog, ref path);

            if (_logFileStream is null)
            {
                _logFileStream = _fileSystem.Open(filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
            }

            var logAsBytes = JsonSerializer.SerializeToUtf8Bytes(messageLog, options.JsonSerializerOptions);

            var isRequest = messageLog is HttpRequestLog;

            if (isRequest)
            {
                await _logFileStream.WriteAsync(UTF8StartBracket, cancellationToken);
            }
            else
            {
                await _logFileStream.WriteAsync(UTF8Enter, cancellationToken);
            }

            await _logFileStream.WriteAsync(logAsBytes, cancellationToken);

            if (!isRequest)
            {
                responseStored = true;

                await _logFileStream.WriteAsync(UTF8EndBracket, cancellationToken);
            }
        }

        private void TryCreateLogDirectory(string path)
        {
            if (!HasLogDirectory)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                HasLogDirectory = true;
            }
        }

        private string GetSavePath(HttpMessageLog messageLog, ref string path)
        {
            var traceId = messageLog.TraceId.Replace(":", "");

            return Path.Combine(path, $"log-{traceId}.json");
        }

        private string GetPath()
        {
            var options = _options.Value;

            return Path.Combine(_environment.ContentRootPath, options.SavePath);
        }


        public void Dispose()
        {
            if (!responseStored)
            {
                _logFileStream.Write(UTF8EndBracket);
            }

            _logFileStream?.Dispose();
        }
    }
}