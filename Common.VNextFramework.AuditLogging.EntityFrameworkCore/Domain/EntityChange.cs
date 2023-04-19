using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.VNextFramework.Auditing;
using Common.VNextFramework.EntityFramework;
using Common.VNextFramework.Tools;

namespace Common.VNextFramework.AuditLogging.EntityFrameworkCore.Domain
{
    public class EntityChange : AuditBaseEntity
    {
        public virtual Guid AuditLogId { get; protected set; }

        public virtual DateTime ChangeTime { get; protected set; }

        public virtual EntityChangeType ChangeType { get; protected set; }

        public virtual string EntityId { get; protected set; }

        public virtual string EntityTypeFullName { get; protected set; }

        public virtual ICollection<EntityPropertyChange> PropertyChanges { get; protected set; }

        


        protected EntityChange()
        {
        }

        public EntityChange(
            Guid auditLogId,
            EntityChangeInfo entityChangeInfo
            )
        {
            Id = GuidTool.GenerateSequentialGuid();
            AuditLogId = auditLogId;
            ChangeTime = entityChangeInfo.ChangeTime;
            ChangeType = entityChangeInfo.ChangeType;
            EntityId = entityChangeInfo.EntityId.Truncate(EntityChangeConsts.MaxEntityTypeFullNameLength);
            EntityTypeFullName =
                entityChangeInfo.EntityTypeFullName.TruncateFromBeginning(
                    EntityChangeConsts.MaxEntityTypeFullNameLength);

            PropertyChanges = entityChangeInfo
                                  .PropertyChanges?
                                  .Select(p => new EntityPropertyChange(Id, p))
                                  .ToList()
                              ?? new List<EntityPropertyChange>();

        }
    }
}
