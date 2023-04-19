using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.VNextFramework.Auditing.AspNetCore;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAuditing(this IApplicationBuilder app)
        {
            return app
                .UseMiddleware<BCChinaAuditingMiddleware>();
        }
    }
}
