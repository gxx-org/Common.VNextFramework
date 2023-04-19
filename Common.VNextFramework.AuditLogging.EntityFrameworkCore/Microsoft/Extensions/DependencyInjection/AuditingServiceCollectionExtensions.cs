using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.VNextFramework.Auditing;
using Common.VNextFramework.AuditLogging.EntityFrameworkCore;
using Common.VNextFramework.EntityFramework.EntityHistory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuditLoginEntityFrameworkCoreServiceCollectionExtensions
    {
        [Obsolete("Please Use UseAuditEntityFrameworkStore to add services")]
        public static IServiceCollection AddAuditLoginEntityFrameworkCore(this IServiceCollection services, Action<EntityFrameworkAuditLoggingOptions> optionAction = null)
        {
            services.Configure<EntityFrameworkAuditLoggingOptions>(options =>
            {
                if (optionAction != null)
                {
                    optionAction(options);
                }
            });
            services.TryAddScoped<IAuditingStore, EntityFrameworkAuditingStore>();
            services.TryAddTransient<IAuditLogRepository, AuditLogRepository>();
            services.TryAddTransient<IEntityHistoryHelper, EntityHistoryHelper>();
            services.TryAddTransient<IAuditLogInfoToAuditLogConverter, AuditLogInfoToAuditLogConverter>();

            return services;
        }


        public static AuditingOptions UseAuditingToEntityFrameworkStore(this AuditingOptions options, Action<EntityFrameworkAuditLoggingOptions> optionAction = null)
        {
            options.Extensions.Add(new EntityFrameworkAuditingStoreOptionsExtension(optionAction));

            return options;
        }
    }
}
