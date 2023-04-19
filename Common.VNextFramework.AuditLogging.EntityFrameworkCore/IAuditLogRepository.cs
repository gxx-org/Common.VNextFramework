using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Common.VNextFramework.Auditing;
using Common.VNextFramework.AuditLogging.EntityFrameworkCore.Domain;

namespace Common.VNextFramework.AuditLogging.EntityFrameworkCore
{
    public interface IAuditLogRepository
    {
        Task AddAsync(AuditLog entity);
    }

    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly AuditLoggingDbContext _auditLoggingDbContext;

        public AuditLogRepository(AuditLoggingDbContext auditLoggingDbContext)
        {
            _auditLoggingDbContext = auditLoggingDbContext;
        }

        public virtual async Task AddAsync(AuditLog entity)
        {
            await _auditLoggingDbContext.AuditLogs.AddAsync(entity);
            await _auditLoggingDbContext.SaveChangesAsync();
        }
    }
}
