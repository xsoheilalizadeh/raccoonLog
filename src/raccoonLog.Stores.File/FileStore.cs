using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace raccoonLog.Stores.File
{
    public class FileStore : IHttpLoggingStore
    {
        private readonly IHostEnvironment _environment;

        private readonly ILogger<FileStore> _logger;

        private readonly IFileSystem _fileSystem;

        private readonly FileStoreOptions _storeOptions;

        private readonly RaccoonLogHttpOptions _options;

        private static readonly byte[] Utf8EndBracket = { (byte)']' };
        private static readonly byte[] Utf8StartBracket = { (byte)'[' };
        private static readonly byte[] Utf8Comma = { (byte)',' };

        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1);

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

            await SemaphoreSlim.WaitAsync();

            try
            {
                await using var fileStream = _fileSystem.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

                bool isBegin = false;    

                if (fileStream.Length == 0)
                {
                    isBegin = true;
                    await fileStream.WriteAsync(Utf8StartBracket, cancellationToken);
                }

                fileStream.Seek(isBegin ? 0 : -1, SeekOrigin.End);

                var lastCharecter = (char)fileStream.ReadByte();

                if (lastCharecter == ']')
                {
                    fileStream.Seek(-1, SeekOrigin.End);

                    await fileStream.WriteAsync(Utf8Comma);
                }

                var logAsBytes = JsonSerializer.SerializeToUtf8Bytes(logContext, _options.JsonSerializerOptions);

                await fileStream.WriteAsync(logAsBytes, cancellationToken);

                await fileStream.WriteAsync(Utf8EndBracket, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }
    }
}
