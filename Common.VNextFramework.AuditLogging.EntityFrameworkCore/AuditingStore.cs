using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.VNextFramework.Auditing;
using Common.VNextFramework.AuditLogging.EntityFrameworkCore.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Common.VNextFramework.AuditLogging.EntityFrameworkCore
{
    public class EntityFrameworkAuditingStore : IAuditingStore
    {
        public ILogger<EntityFrameworkAuditingStore> Logger { get; set; }

        private readonly IAuditLogRepository _auditLogRepository;

        protected IAuditLogInfoToAuditLogConverter Converter { get; }

        protected AuditingOptions Options { get; }

        protected EntityFrameworkAuditLoggingOptions AuditLoggingOptions;

        public EntityFrameworkAuditingStore(
            IAuditLogInfoToAuditLogConverter converter ,
            ILoggerFactory loggerFactory, 
            IOptions<AuditingOptions> options, 
            IOptions<EntityFrameworkAuditLoggingOptions> auditLoggingOptions, IAuditLogRepository auditLogRepository)
        {
            Converter = converter;
            _auditLogRepository = auditLogRepository;
            AuditLoggingOptions = auditLoggingOptions.Value;
            Options = options.Value;
            Logger = loggerFactory.CreateLogger<EntityFrameworkAuditingStore>();
        }

        public async Task SaveAsync(AuditLogInfo auditInfo)
        {
            if (!Options.HideErrors)
            {
                await SaveLogAsync(auditInfo);
                return;
            }

            try
            {
                await SaveLogAsync(auditInfo);
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Could not save the audit log object: " + Environment.NewLine + auditInfo);
                Logger.LogError(ex.Message, LogLevel.Error);
            }
        }


        protected virtual async Task SaveLogAsync(AuditLogInfo auditInfo)
        {
            var entity = await Converter.ConvertAsync(auditInfo);
            if (!AuditLoggingOptions.SaveActions)
            {
                entity.Actions.Clear();
            }

            if (!AuditLoggingOptions.SaveIfNoEntityChanges && entity.EntityChanges.Count == 0)
            {
                return;
            }

            await _auditLogRepository.AddAsync(entity);
        }
    }
}
