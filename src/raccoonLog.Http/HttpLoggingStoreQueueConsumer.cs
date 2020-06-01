using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace raccoonLog.Http
{
    public class StoreQueueConsumer : IHostedService
    {
        private readonly IStoreQueue _storeQueue;

        private ILogger<StoreQueueConsumer> _logger;

        private Timer _timer;

        public StoreQueueConsumer(IStoreQueue storeQueue, ILogger<StoreQueueConsumer> logger)
        {
            _storeQueue = storeQueue;
            _logger = logger;
            _timer = new Timer(500);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer.AutoReset = true;
            _timer.Elapsed += (sender, e) =>
            {
                try
                {
                    _storeQueue.Dequeue().GetAwaiter().GetResult();
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, exception.Message);
                }
            };

            _timer.Start();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Dispose();
            return Task.CompletedTask;
        }
    }
}
