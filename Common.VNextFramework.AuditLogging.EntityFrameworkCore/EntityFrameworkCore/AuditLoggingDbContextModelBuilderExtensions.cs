using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Common.VNextFramework.AuditLogging.EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Common.VNextFramework.EntityFramework;

namespace Common.VNextFramework.AuditLogging.EntityFrameworkCore
{
    public static class AuditLoggingDbContextModelBuilderExtensions
    {
        public static void ConfigureAuditLogging(
            [NotNull] this ModelBuilder builder,
            Action<AuditLoggingModelBuilderConfigurationOptions> optionsAction = null)
        {

            var options = new AuditLoggingModelBuilderConfigurationOptions()
            {
                EntityPropertyChangeToJson = true,
                Schema = "AuditLogs"
            };

            optionsAction?.Invoke(options);

            builder.Entity<AuditLog>(b =>
            {
                b.ToTable(options.TablePrefix + "AuditLogs", options.Schema);

                b.Property(x => x.Id);
                b.Property(x => x.ApplicationName).HasMaxLength(AuditLogConsts.MaxApplicationNameLength).HasColumnName(nameof(AuditLog.ApplicationName));
                b.Property(x => x.ClientIpAddress).HasMaxLength(AuditLogConsts.MaxClientIpAddressLength).HasColumnName(nameof(AuditLog.ClientIpAddress));
                b.Property(x => x.ClientName).HasMaxLength(AuditLogConsts.MaxClientNameLength).HasColumnName(nameof(AuditLog.ClientName));
                b.Property(x => x.ClientId).HasMaxLength(AuditLogConsts.MaxClientIdLength).HasColumnName(nameof(AuditLog.ClientId));
                b.Property(x => x.CorrelationId).HasMaxLength(AuditLogConsts.MaxCorrelationIdLength).HasColumnName(nameof(AuditLog.CorrelationId));
                b.Property(x => x.BrowserInfo).HasMaxLength(AuditLogConsts.MaxBrowserInfoLength).HasColumnName(nameof(AuditLog.BrowserInfo));
                b.Property(x => x.HttpMethod).HasMaxLength(AuditLogConsts.MaxHttpMethodLength).HasColumnName(nameof(AuditLog.HttpMethod));
                b.Property(x => x.Url).HasMaxLength(AuditLogConsts.MaxUrlLength).HasColumnName(nameof(AuditLog.Url));
                b.Property(x => x.HttpStatusCode).HasColumnName(nameof(AuditLog.HttpStatusCode));

                b.Property(x => x.Comments).HasMaxLength(AuditLogConsts.MaxCommentsLength).HasColumnName(nameof(AuditLog.Comments));
                b.Property(x => x.ExecutionDuration).HasColumnName(nameof(AuditLog.ExecutionDuration));
                b.Property(x => x.ImpersonatorTenantId).HasColumnName(nameof(AuditLog.ImpersonatorTenantId));
                b.Property(x => x.ImpersonatorUserId).HasColumnName(nameof(AuditLog.ImpersonatorUserId));
                b.Property(x => x.UserId).HasColumnName(nameof(AuditLog.UserId));
                b.Property(x => x.UserName).HasMaxLength(AuditLogConsts.MaxUserNameLength).HasColumnName(nameof(AuditLog.UserName));

                b.HasMany(a => a.Actions).WithOne().HasForeignKey(x => x.AuditLogId).IsRequired();
                b.HasMany(a => a.EntityChanges).WithOne().HasForeignKey(x => x.AuditLogId).IsRequired();
            });

            builder.Entity<AuditLogAction>(b =>
            {
                b.ToTable(options.TablePrefix + "AuditLogActions", options.Schema);

                b.Property(x => x.Id);

                b.Property(x => x.AuditLogId).HasColumnName(nameof(AuditLogAction.AuditLogId));
                b.Property(x => x.ServiceName).HasMaxLength(AuditLogActionConsts.MaxServiceNameLength).HasColumnName(nameof(AuditLogAction.ServiceName));
                b.Property(x => x.MethodName).HasMaxLength(AuditLogActionConsts.MaxMethodNameLength).HasColumnName(nameof(AuditLogAction.MethodName));
                b.Property(x => x.Parameters).HasMaxLength(AuditLogActionConsts.MaxParametersLength).HasColumnName(nameof(AuditLogAction.Parameters));
                b.Property(x => x.ExecutionTime).HasColumnName(nameof(AuditLogAction.ExecutionTime));
                b.Property(x => x.ExecutionDuration).HasColumnName(nameof(AuditLogAction.ExecutionDuration));

                b.HasIndex(x => new { x.AuditLogId });
                b.HasIndex(x => new { x.ServiceName, x.MethodName, x.ExecutionTime });

            });

            builder.Entity<EntityChange>(b =>
            {
                b.ToTable(options.TablePrefix + "EntityChanges", options.Schema);

                b.Property(x => x.Id);
                b.Property(x => x.EntityTypeFullName).HasMaxLength(EntityChangeConsts.MaxEntityTypeFullNameLength).IsRequired().HasColumnName(nameof(EntityChange.EntityTypeFullName));
                b.Property(x => x.EntityId).HasMaxLength(EntityChangeConsts.MaxEntityIdLength).IsRequired().HasColumnName(nameof(EntityChange.EntityId));
                b.Property(x => x.AuditLogId).IsRequired().HasColumnName(nameof(EntityChange.AuditLogId));
                b.Property(x => x.ChangeTime).IsRequired().HasColumnName(nameof(EntityChange.ChangeTime));
                b.Property(x => x.ChangeType).IsRequired().HasColumnName(nameof(EntityChange.ChangeType));

                if (options.EntityPropertyChangeToJson)
                {
                    b.Property(x => x.PropertyChanges).HasJsonConversion();
                }
                else
                {
                    b.HasMany(a => a.PropertyChanges).WithOne().HasForeignKey(x => x.EntityChangeId);
                }


                b.HasIndex(x => new { x.AuditLogId });
                b.HasIndex(x => new { x.EntityTypeFullName, x.EntityId });

            });

            if (!options.EntityPropertyChangeToJson)
            {
                builder.Entity<EntityPropertyChange>(b =>
                {
                    b.ToTable(options.TablePrefix + "EntityPropertyChanges", options.Schema);
                    b.Property(x => x.Id);

                    b.Property(x => x.NewValue).HasColumnName(nameof(EntityPropertyChange.NewValue));
                    b.Property(x => x.PropertyName).IsRequired().HasColumnName(nameof(EntityPropertyChange.PropertyName));
                    b.Property(x => x.PropertyTypeFullName).IsRequired().HasColumnName(nameof(EntityPropertyChange.PropertyTypeFullName));
                    b.Property(x => x.OriginalValue).HasColumnName(nameof(EntityPropertyChange.OriginalValue));

                    b.HasIndex(x => new { x.EntityChangeId });

                });
            }

        }
    }
}
