using System.Collections.Generic;

namespace Common.VNextFramework.Auditing.AspNetCore
{
    public class AspNetCoreAuditingOptions
    {
        public List<string> IgnoredUrls { get; } = new List<string>();
    }
}
