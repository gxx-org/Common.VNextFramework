using System;
using System.Collections.Generic;
using System.Text;

namespace Common.VNextFramework.AuditLogging.EntityFrameworkCore
{
    public class AuditLoggingModelBuilderConfigurationOptions
    {
        public string TablePrefix { get; set; }

        public string Schema { get; set; }

        public bool EntityPropertyChangeToJson { get; set; }

        public AuditLoggingModelBuilderConfigurationOptions(
            string tablePrefix = "",
             string schema = null)
        {
            TablePrefix = tablePrefix;
            Schema = schema;
        }
    }
}
