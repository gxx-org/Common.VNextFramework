using System;
using System.Collections.Generic;
using System.Text;
using Common.VNextFramework.Auditing;
using Common.VNextFramework.EntityFramework;

namespace Common.VNextFramework.AuditLogging.EntityFrameworkCore.Domain
{
    public class AuditLogAction : AuditBaseEntity
    {
        public virtual Guid AuditLogId { get; protected set; }

        public virtual string ServiceName { get; protected set; }

        public virtual string MethodName { get; protected set; }

        public virtual string Parameters { get; protected set; }

        public virtual DateTime ExecutionTime { get; protected set; }

        public virtual int ExecutionDuration { get; protected set; }


        protected AuditLogAction()
        {
        }

        public AuditLogAction(Guid id, Guid auditLogId, AuditLogActionInfo actionInfo)
        {

            Id = id;
            AuditLogId = auditLogId;
            ExecutionTime = actionInfo.ExecutionTime;
            ExecutionDuration = actionInfo.ExecutionDuration;
            ServiceName = actionInfo.ServiceName.TruncateFromBeginning(AuditLogActionConsts.MaxServiceNameLength);
            MethodName = actionInfo.MethodName.TruncateFromBeginning(AuditLogActionConsts.MaxMethodNameLength);
            Parameters = actionInfo.Parameters.Length > AuditLogActionConsts.MaxParametersLength ? "" : actionInfo.Parameters;
        }
    }
}
