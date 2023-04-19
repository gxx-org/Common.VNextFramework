using System;
using System.Collections.Generic;
using System.Text;
using Common.VNextFramework.AuditLogging.EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Common.VNextFramework.AuditLogging.EntityFrameworkCore
{
    public class AuditLoggingDbContext : DbContext
    {
        public AuditLoggingDbContext(DbContextOptions<AuditLoggingDbContext> options) : base(options)
        {
        }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<EntityChange> EntityChanges { get; set; }

        public DbSet<AuditLogAction> AuditLogActions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureAuditLogging();
        }
    }
}
