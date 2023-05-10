using System;
using System.Threading.Tasks;
using System.Threading;

namespace Common.VNextFramework.DistributedLock.Abstractions
{
    public interface ICustomDistributedLock
    {
        public Task<ICustomDistributedLockHandle> TryAcquireAsync(string key, TimeSpan timeout = default, CancellationToken cancellationToken = default);
        public Task<ICustomDistributedLockHandle> AcquireAsync(string key, TimeSpan? timeout = null, CancellationToken cancellationToken = default);
    }
}
