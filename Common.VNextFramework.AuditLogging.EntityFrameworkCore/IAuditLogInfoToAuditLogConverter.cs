using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.VNextFramework.Auditing;
using Common.VNextFramework.AuditLogging.EntityFrameworkCore.Domain;
using Common.VNextFramework.Tools;
using Newtonsoft.Json;

namespace Common.VNextFramework.AuditLogging.EntityFrameworkCore
{
    public interface IAuditLogInfoToAuditLogConverter
    {
        Task<AuditLog> ConvertAsync(AuditLogInfo auditLogInfo);
    }

    public class AuditLogInfoToAuditLogConverter : IAuditLogInfoToAuditLogConverter
    {
        public virtual Task<AuditLog> ConvertAsync(AuditLogInfo auditLogInfo)
        {
            var auditLogId = GuidTool.GenerateSequentialGuid();

            var entityChanges = auditLogInfo
                                    .EntityChanges?
                                    .Select(entityChangeInfo => new EntityChange(auditLogId, entityChangeInfo))
                                    .ToList()
                                ?? new List<EntityChange>();

            var actions = auditLogInfo
                              .Actions?
                              .Select(auditLogActionInfo => new AuditLogAction(GuidTool.GenerateSequentialGuid(), auditLogId, auditLogActionInfo))
                              .ToList()
                          ?? new List<AuditLogAction>();

            var exceptions = JsonConvert.SerializeObject(auditLogInfo.Exceptions ?? new List<Exception>());

            var comments = string.Join(Environment.NewLine, auditLogInfo
                .Comments ?? new List<string>());

            var auditLog = new AuditLog(
                auditLogId,
                auditLogInfo.ApplicationName,
                auditLogInfo.UserId,
                auditLogInfo.UserName,
                auditLogInfo.ExecutionTime,
                auditLogInfo.ExecutionDuration,
                auditLogInfo.ClientIpAddress,
                auditLogInfo.ClientName,
                auditLogInfo.ClientId,
                auditLogInfo.CorrelationId,
                auditLogInfo.BrowserInfo,
                auditLogInfo.HttpMethod,
                auditLogInfo.Url,
                auditLogInfo.HttpStatusCode,
                auditLogInfo.ImpersonatorUserId,
                entityChanges,
                actions,
                exceptions,
                comments
            );

            return Task.FromResult(auditLog);
        }
    }
}
