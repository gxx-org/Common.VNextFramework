using System;
using Common.VNextFramework.Auditing;
using Common.VNextFramework.Auditing.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuditing(this IServiceCollection services, Action<AuditingOptions> setupAction = null)
        {
            var options = new AuditingOptions();
            options.Contributors.Add(new AspNetCoreAuditLogContributor());
            setupAction?.Invoke(options);

            services.Configure<AuditingOptions>(x => x.Contributors.Add(new AspNetCoreAuditLogContributor()));
            services.Configure(setupAction);

            foreach (var auditingOptionsExtension in options.Extensions)
            {
                auditingOptionsExtension.AddServices(services);
            }

            services.Configure<MvcOptions>(x => x.Filters.AddService<AuditActionFilter>());
            services.AddTransient<AuditActionFilter>();

            services.AddTransient<BCChinaAuditingMiddleware>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IAuditingFactory, AuditingFactory>();
            services.TryAddTransient<IAuditingManager, AuditingManager>();
            services.TryAddSingleton<IAuditingHelper, AuditingHelper>();
            services.TryAddSingleton<IAuditSerializer, JsonAuditSerializer>();

            return services;
        }

    }
}
