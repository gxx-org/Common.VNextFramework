using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Common.VNextFramework.DistributedLock.Abstractions
{
    public class LocalDistributedLock : ICustomDistributedLock
    {
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> Locker =
            new ConcurrentDictionary<string, SemaphoreSlim>();

        private readonly ILogger<LocalDistributedLock> _logger;

        public LocalDistributedLock(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<LocalDistributedLock>();
        }

        public async Task<ICustomDistributedLockHandle> TryAcquireAsync(string key, TimeSpan timeout = default, CancellationToken cancellationToken = default)
        {
            Check.NotNullOrWhiteSpace(key, nameof(key));

            var semaphore = Locker.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));

            if (!await semaphore.WaitAsync(timeout, cancellationToken))
            {
                return null;
            }

            return new LocalAbpDistributedLockHandle(semaphore);
        }

        public async Task<ICustomDistributedLockHandle> AcquireAsync(string key, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            var semaphore = Locker.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));

            await semaphore.WaitAsync(cancellationToken);
            return new LocalAbpDistributedLockHandle(semaphore);
        }
    }
}
