using System;
using Common.VNextFramework.Auditing;
using Common.VNextFramework.AuditLogging.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public class EntityFrameworkAuditingStoreOptionsExtension : IAuditingOptionsExtension
    {
        private readonly Action<EntityFrameworkAuditLoggingOptions> _optionAction;

        public EntityFrameworkAuditingStoreOptionsExtension(Action<EntityFrameworkAuditLoggingOptions> optionAction = null)
        {
            _optionAction = optionAction;
        }

        public IServiceCollection AddServices(IServiceCollection services)
        {
            services.Configure<EntityFrameworkAuditLoggingOptions>(options =>
            {
                if (_optionAction != null)
                {
                    _optionAction(options);
                }
            });

            services.AddScoped<IAuditingStore, EntityFrameworkAuditingStore>();
            services.TryAddTransient<IAuditLogRepository, AuditLogRepository>();
            services.TryAddTransient<IAuditLogInfoToAuditLogConverter, AuditLogInfoToAuditLogConverter>();
            return services;
        }
    }
}