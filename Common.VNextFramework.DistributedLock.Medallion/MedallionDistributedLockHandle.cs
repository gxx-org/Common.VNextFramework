using Common.VNextFramework.DistributedLock.Abstractions;
using Medallion.Threading;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.VNextFramework.DistributedLock.Medallion
{
    public class MedallionDistributedLockHandle : ICustomDistributedLockHandle
    {
        public IDistributedSynchronizationHandle Handle { get; }

        public MedallionDistributedLockHandle(IDistributedSynchronizationHandle handle)
        {
            Handle = handle;
        }

        public ValueTask DisposeAsync()
        {
            return Handle.DisposeAsync();
        }
    }
}
