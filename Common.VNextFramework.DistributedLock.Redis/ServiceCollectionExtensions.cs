using System;
using BCChina.VNextFramework.DistributedLock.Redis;
using Common.VNextFramework.DistributedLock.Abstractions;
using Medallion.Threading.Redis;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisDistributedLock(this IServiceCollection services, string connectionString)
        {
            var connection = ConnectionMultiplexer.Connect(connectionString);
            services.AddSingleton(new RedisDistributedSynchronizationProvider(connection.GetDatabase()));
            services.TryAddSingleton<ICustomDistributedLock, RedisMedallionRedisDistributedLock>();

            return services;
        }

        public static IServiceCollection AddRedisDistributedLock(this IServiceCollection services, Action<IServiceProvider,RedisDistributedSynchronizationProvider> action)
        {
            services.AddSingleton(action);
            services.TryAddSingleton<ICustomDistributedLock, RedisMedallionRedisDistributedLock>();

            return services;
        }
    }
}
