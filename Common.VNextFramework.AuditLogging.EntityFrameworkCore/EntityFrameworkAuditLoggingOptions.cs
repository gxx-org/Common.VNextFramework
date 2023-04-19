using System;
using System.Collections.Generic;
using System.Text;

namespace Common.VNextFramework.AuditLogging.EntityFrameworkCore
{
    public class EntityFrameworkAuditLoggingOptions
    {
        public bool SaveActions { get; set; } = true;

        public bool SaveIfNoEntityChanges { get; set; } = true;
    }
}
