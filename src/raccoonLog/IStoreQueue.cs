using System;
using System.Threading.Tasks;

namespace raccoonLog
{
    public interface IStoreQueue
    {
        Task Dequeue();

        void Enqueue(Func<Task> storeTask);
    }
}