using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Common.VNextFramework.DistributedLock.Abstractions
{
    public class LocalAbpDistributedLockHandle : ICustomDistributedLockHandle
    {
        private readonly SemaphoreSlim _semaphore;

        public LocalAbpDistributedLockHandle(SemaphoreSlim semaphore)
        {
            _semaphore = semaphore;
        }

        public ValueTask DisposeAsync()
        {
            _semaphore.Release();
            return default;
        }
    }
}
