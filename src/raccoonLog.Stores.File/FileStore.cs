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

        private FileStream _logFileStream;

        private IFileSystem _fileSystem;

        private readonly FileStoreOptions _storeOptions;

        private readonly RaccoonLogHttpOptions _options;

        private static readonly byte[] UTF8EndBracket = { (byte)']' };
        private static readonly byte[] UTF8StartBracket = { (byte)'[' };
        private static readonly byte[] UTF8Comma = { (byte)',' };

        public FileStore(IHostEnvironment environment,
            IOptions<RaccoonLogHttpOptions> logHttpOptions,
            IOptions<FileStoreOptions> options,
            IFileSystem fileSystem
        )
        {
            _environment = environment;
            _storeOptions = options.Value;
            _fileSystem = fileSystem;
            _options = logHttpOptions.Value;
        }

        public string DirectoryPath => Path.Combine(_environment.ContentRootPath, _storeOptions.SavePath);

        public async ValueTask StoreAsync(LogContext logContext, CancellationToken cancellationToken = default)
        {
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }

            var filePath = Path.Combine(DirectoryPath, $"{_storeOptions.FileName}");

            _logFileStream = _fileSystem.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

            if (_logFileStream.Length == 0)
            {
                await _logFileStream.WriteAsync(UTF8StartBracket);
            }

            _logFileStream.Seek(_logFileStream.Length == 0 ? 0 : -1, SeekOrigin.End);

            var lastCharecter = (char)_logFileStream.ReadByte();

            if (lastCharecter == ']')
            {
                _logFileStream.Seek(-1, SeekOrigin.End);

                await _logFileStream.WriteAsync(UTF8Comma);
            }

            var logAsBytes = JsonSerializer.SerializeToUtf8Bytes(logContext, _options.JsonSerializerOptions);

            await _logFileStream.WriteAsync(logAsBytes, cancellationToken);

            await _logFileStream.WriteAsync(UTF8EndBracket);
        }

        public void Dispose()
        {

            _logFileStream?.Dispose();
        }
    }
}
