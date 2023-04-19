using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.VNextFramework.Auditing;
using Common.VNextFramework.AuditLogging.EntityFrameworkCore;
using Common.VNextFramework.EntityFramework.EntityHistory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuditLoginEntityFrameworkCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddAuditLoggingDbContextEntityFrameworkCoreSqlServer(this IServiceCollection services, string connectionString, bool autoMigrations = false)
        {
            services.AddDbContext<AuditLoggingDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Common.VNextFramework.AuditLogging.EntityFrameworkCore.SqlServer")));

            if (autoMigrations)
            {
                var serviceProvider = services.BuildServiceProvider();
                using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope()) 
                {
                    var dbContext = serviceScope.ServiceProvider.GetService<AuditLoggingDbContext>(); 
                    serviceScope.ServiceProvider.GetService<AuditLoggingDbContext>()?.Database.Migrate();
                }
            }

            return services;
        }


    }
}
