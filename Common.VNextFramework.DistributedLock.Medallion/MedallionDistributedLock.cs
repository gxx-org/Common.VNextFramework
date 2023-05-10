using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Common.VNextFramework.DistributedLock.Abstractions;
using Medallion.Threading;

namespace Common.VNextFramework.DistributedLock.Medallion
{
    public class MedallionDistributedLock : ICustomDistributedLock
    {
        protected IDistributedLockProvider DistributedLockProvider { get; }

        public MedallionDistributedLock(IDistributedLockProvider distributedLockProvider)
        {
            DistributedLockProvider = distributedLockProvider;
        }

        public async Task<ICustomDistributedLockHandle> TryAcquireAsync(
            string key,
            TimeSpan timeout = default,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrWhiteSpace(key, nameof(key));

            var handle = await DistributedLockProvider.TryAcquireLockAsync(key, timeout, cancellationToken);
            if (handle == null)
            {
                return null;
            }

            return new MedallionDistributedLockHandle(handle);
        }

        public async Task<ICustomDistributedLockHandle> AcquireAsync(string key, TimeSpan? timeout = null,
            CancellationToken cancellationToken = default)
        {
            var handle = await DistributedLockProvider.AcquireLockAsync(key, timeout, cancellationToken);
            return new MedallionDistributedLockHandle(handle);
        }
    }
}

