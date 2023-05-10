using Common.VNextFramework.DistributedLock.Abstractions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLocalDistributedLock(this IServiceCollection services)
        {
            services.TryAddSingleton<ICustomDistributedLock, LocalDistributedLock>();

            return services;
        }
    }
}
