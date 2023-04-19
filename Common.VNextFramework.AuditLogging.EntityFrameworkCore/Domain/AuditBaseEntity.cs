using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Common.VNextFramework.EntityFramework;

namespace Common.VNextFramework.AuditLogging.EntityFrameworkCore.Domain
{
    //TODO: 略丑陋 ，找个时间重构下GuidEntity 当前 不需要依赖 GuidEntity
    public abstract class AuditBaseEntity : GuidEntity
    {
        [NotMapped]
        public override string CreatedBy { get; set; }

        public override DateTimeOffset? CreatedOn { get; set; } = DateTimeOffset.Now;

        [NotMapped]
        public override bool IsDeleted { get; set; }

        [NotMapped]
        public override byte[] Timestamp { get; set; }

        [NotMapped]
        public override string LastModifiedBy { get; set; }

        [NotMapped]
        public override DateTimeOffset? LastModifiedOn { get; set; }
    }
}
