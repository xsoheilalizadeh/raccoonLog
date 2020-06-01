using System;
using Microsoft.Extensions.Options;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace raccoonLog.Http.Stores
{
    public class FileStore : IHttpLoggingStore
    {
        private readonly IHostEnvironment _environment;

        private readonly ILogger<FileStore> _logger;

        private IFileSystem _fileSystem;

        private readonly FileStoreOptions _storeOptions;

        private readonly RaccoonLogHttpOptions _options;

        private static readonly byte[] UTF8EndBracket = { (byte)']' };
        private static readonly byte[] UTF8StartBracket = { (byte)'[' };
        private static readonly byte[] UTF8Comma = { (byte)',' };

        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);

        public FileStore(IHostEnvironment environment,
            IOptions<RaccoonLogHttpOptions> logHttpOptions,
            IOptions<FileStoreOptions> options,
            IFileSystem fileSystem
, ILogger<FileStore> logger)
        {
            _environment = environment;
            _storeOptions = options.Value;
            _fileSystem = fileSystem;
            _options = logHttpOptions.Value;
            _logger = logger;
        }

        public string DirectoryPath => Path.Combine(_environment.ContentRootPath, _storeOptions.SavePath);

        public async Task StoreAsync(LogContext logContext, CancellationToken cancellationToken = default)
        {
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }

            var filePath = Path.Combine(DirectoryPath, $"{_storeOptions.FileName}");

            await _semaphoreSlim.WaitAsync();

            try
            {
                using var _fileStream = _fileSystem.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

                bool isBegin = false;

                if (_fileStream.Length == 0)
                {
                    isBegin = true;
                    await _fileStream.WriteAsync(UTF8StartBracket, cancellationToken);
                }

                _fileStream.Seek(isBegin ? 0 : -1, SeekOrigin.End);

                var lastCharecter = (char)_fileStream.ReadByte();

                if (lastCharecter == ']')
                {
                    _fileStream.Seek(-1, SeekOrigin.End);

                    await _fileStream.WriteAsync(UTF8Comma);
                }

                var logAsBytes = JsonSerializer.SerializeToUtf8Bytes(logContext, _options.JsonSerializerOptions);

                await _fileStream.WriteAsync(logAsBytes, cancellationToken);

                await _fileStream.WriteAsync(UTF8EndBracket, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
    }
}
