using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Common.VNextFramework.EntityFramework;
using Common.VNextFramework.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EfRepositoryCollectionExtensions
    {
        public static IServiceCollection EfRepositoryRegistrar<TDbContext>(this IServiceCollection service) where TDbContext : DbContext
        {
            var entityTypes = GetEntityTypes(typeof(TDbContext));
            foreach (var entityType in entityTypes)
            {
                var interfaceRepositoryType = typeof(IEFAsyncRepository<>).MakeGenericType(entityType);
                var repositoryType = typeof(EFRepository<>).MakeGenericType(entityType);
                service.Add(new ServiceDescriptor(interfaceRepositoryType, service =>
                {
                    var dbContext = service.GetRequiredService<TDbContext>();
                    return Activator.CreateInstance(repositoryType, dbContext);
                }, ServiceLifetime.Scoped));
            }

            return service;
        }

        internal static IEnumerable<Type> GetEntityTypes(Type dbContextType)
        {
            return
                from property in dbContextType.GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where
                    ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(DbSet<>)) &&
                    typeof(IEntity).IsAssignableFrom(property.PropertyType.GenericTypeArguments[0])
                select property.PropertyType.GenericTypeArguments[0];
        }
    }
}
