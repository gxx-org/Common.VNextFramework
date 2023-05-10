using Common.VNextFramework.DistributedLock.Medallion;
using Medallion.Threading.Redis;

namespace BCChina.VNextFramework.DistributedLock.Redis
{
    public class RedisMedallionRedisDistributedLock : MedallionDistributedLock
    {
        public RedisMedallionRedisDistributedLock(RedisDistributedSynchronizationProvider distributedLockProvider) : base(distributedLockProvider)
        {
        }
    }
}
