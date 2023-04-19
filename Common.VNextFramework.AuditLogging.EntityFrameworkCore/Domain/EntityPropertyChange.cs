using System;
using System.Collections.Generic;
using System.Text;
using Common.VNextFramework.Auditing;
using Common.VNextFramework.EntityFramework;
using Common.VNextFramework.Tools;
using Newtonsoft.Json;

namespace Common.VNextFramework.AuditLogging.EntityFrameworkCore.Domain
{
    public class EntityPropertyChange :GuidEntity
    {
        [JsonIgnore]
        public override Guid Id { get; set; }

        [JsonIgnore]
        public virtual Guid EntityChangeId { get; protected set; }

        [JsonProperty("n", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string NewValue { get; protected set; }

        [JsonProperty("o", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string OriginalValue { get; protected set; }

        [JsonProperty("p")]
        public virtual string PropertyName { get; protected set; }

        [JsonIgnore]
        public virtual string PropertyTypeFullName { get; protected set; }

        [JsonIgnore]
        public override DateTimeOffset? CreatedOn { get; set; }
        [JsonIgnore]
        public override string CreatedBy { get; set; }
        [JsonIgnore]
        public override DateTimeOffset? LastModifiedOn { get; set; }
        [JsonIgnore]
        public override string LastModifiedBy { get; set; }
        [JsonIgnore]
        public override bool IsDeleted { get; set; }
        [JsonIgnore]
        public override byte[] Timestamp { get; set; }

        protected EntityPropertyChange()
        {

        }

        public EntityPropertyChange(
            Guid entityChangeId,
            EntityPropertyChangeInfo entityChangeInfo
           )
        {

            EntityChangeId = entityChangeId;
            //NewValue = entityChangeInfo.NewValue.Truncate(EntityPropertyChangeConsts.MaxNewValueLength);
            //OriginalValue = entityChangeInfo.OriginalValue.Truncate(EntityPropertyChangeConsts.MaxOriginalValueLength);
            //PropertyName = entityChangeInfo.PropertyName.TruncateFromBeginning(EntityPropertyChangeConsts.MaxPropertyNameLength);
            //PropertyTypeFullName = entityChangeInfo.PropertyTypeFullName.TruncateFromBeginning(EntityPropertyChangeConsts.MaxPropertyTypeFullNameLength);
            if (entityChangeInfo == null)
            {
                return;
            }
            NewValue = entityChangeInfo.NewValue;
            OriginalValue = entityChangeInfo.OriginalValue;
            PropertyName = entityChangeInfo.PropertyName;
            PropertyTypeFullName = entityChangeInfo.PropertyTypeFullName;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
