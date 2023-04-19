using System;
using System.Collections.Generic;
using System.Text;
using Common.VNextFramework.EntityFramework;

namespace Common.VNextFramework.AuditLogging.EntityFrameworkCore.Domain
{
    public class AuditLog: AuditBaseEntity
    {
        public virtual string ApplicationName { get; set; }

        public virtual Guid? UserId { get; protected set; }

        public virtual string UserName { get; protected set; }

        public virtual Guid? ImpersonatorUserId { get; protected set; }

        public virtual Guid? ImpersonatorTenantId { get; protected set; }

        public virtual DateTime ExecutionTime { get; protected set; }

        public virtual int ExecutionDuration { get; protected set; }

        public virtual string ClientIpAddress { get; protected set; }

        public virtual string ClientName { get; protected set; }

        public virtual string ClientId { get; set; }

        public virtual string CorrelationId { get; set; }

        public virtual string BrowserInfo { get; protected set; }

        public virtual string HttpMethod { get; protected set; }

        public virtual string Url { get; protected set; }

        public virtual string Exceptions { get; protected set; }

        public virtual string Comments { get; protected set; }

        public virtual int? HttpStatusCode { get; set; }

        public virtual ICollection<EntityChange> EntityChanges { get; protected set; }

        public virtual ICollection<AuditLogAction> Actions { get; protected set; }

        protected AuditLog()
        {

        }

        public AuditLog(
           Guid id,
           string applicationName,
           Guid? userId,
           string userName,
           DateTime executionTime,
           int executionDuration,
           string clientIpAddress,
           string clientName,
           string clientId,
           string correlationId,
           string browserInfo,
           string httpMethod,
           string url,
           int? httpStatusCode,
           Guid? impersonatorUserId,
           List<EntityChange> entityChanges,
           List<AuditLogAction> actions,
           string exceptions,
           string comments)
        {
            Id = id;
            ApplicationName = applicationName.Truncate(AuditLogConsts.MaxApplicationNameLength);
            UserId = userId;
            UserName = userName.Truncate(AuditLogConsts.MaxUserNameLength);
            ExecutionTime = executionTime;
            ExecutionDuration = executionDuration;
            ClientIpAddress = clientIpAddress.Truncate(AuditLogConsts.MaxClientIpAddressLength);
            ClientName = clientName.Truncate(AuditLogConsts.MaxClientNameLength);
            ClientId = clientId.Truncate(AuditLogConsts.MaxClientIdLength);
            CorrelationId = correlationId.Truncate(AuditLogConsts.MaxCorrelationIdLength);
            BrowserInfo = browserInfo.Truncate(AuditLogConsts.MaxBrowserInfoLength);
            HttpMethod = httpMethod.Truncate(AuditLogConsts.MaxHttpMethodLength);
            Url = url.Truncate(AuditLogConsts.MaxUrlLength);
            HttpStatusCode = httpStatusCode;
            ImpersonatorUserId = impersonatorUserId;

            EntityChanges = entityChanges;
            Actions = actions;
            Exceptions = exceptions;
            Comments = comments.Truncate(AuditLogConsts.MaxCommentsLength);
        }

    }
}
