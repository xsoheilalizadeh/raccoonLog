using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace raccoonLog
{
    public class StoreQueue : IStoreQueue
    {
        private readonly ILogger<StoreQueue> _logger;
        private readonly ConcurrentQueue<Func<Task>> _queue = new ConcurrentQueue<Func<Task>>();

        public StoreQueue(ILogger<StoreQueue> logger)
        {
            _logger = logger;
        }

        public async Task Dequeue()
        {
            if (_queue.TryDequeue(out var storeTask))
            {
                _logger.LogInformation("1 task dequeued, {count} remained", _queue.Count);

                await storeTask();
            }
        }

        public void Enqueue(Func<Task> storeTask)
        {
            _queue.Enqueue(storeTask);
        }
    }
}