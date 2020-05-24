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
        private readonly IHostEnvironment _environment;

        private static bool HasLogDirectory;

        private FileStream _logFileStream;

        private IFileSystem _fileSystem;

        private readonly FileStoreOptions _storeOptions;

        private readonly RaccoonLogHttpOptions _options;

        internal FileStore(
            IOptions<RaccoonLogHttpOptions> logHttpOptions,
            IOptions<FileStoreOptions> options,
            IFileSystem fileSystem
        )
        {
            _storeOptions = options.Value;
            _fileSystem = fileSystem;
            _options = logHttpOptions.Value;
        }


        public FileStore(IHostEnvironment environment,
            IOptions<RaccoonLogHttpOptions> logHttpOptions,
            IOptions<FileStoreOptions> options,
            IFileSystem fileSystem
        ) : this(logHttpOptions, options, fileSystem)
        {
            _environment = environment;
        }

        public string DirectoryPath => Path.Combine(_environment.ContentRootPath, _storeOptions.SavePath);

        public async ValueTask StoreAsync(LogContext logContext, CancellationToken cancellationToken = default)
        {
            TryCreateLogDirectory();

            var filePath = GetSavePath(logContext.TraceId);

            if (_logFileStream is null)
            {
                _logFileStream = _fileSystem.Open(filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
            }

            var logAsBytes = JsonSerializer.SerializeToUtf8Bytes(logContext, _options.JsonSerializerOptions);

            await _logFileStream.WriteAsync(logAsBytes, cancellationToken);
        }


        private void TryCreateLogDirectory()
        {
            if (!HasLogDirectory)
            {
                if (!Directory.Exists(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath);
                }

                HasLogDirectory = true;
            }
        }

        private string GetSavePath(string traceId)
        {
            _ = traceId ?? throw new ArgumentNullException(nameof(traceId));

            traceId = traceId.Replace(":", "");

            return Path.Combine(DirectoryPath, $"log-{traceId}.json");
        }

        public void Dispose()
        {
            _logFileStream?.Dispose();
        }
    }
}