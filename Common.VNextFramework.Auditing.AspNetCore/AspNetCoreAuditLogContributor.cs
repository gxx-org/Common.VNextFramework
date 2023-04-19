using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Common.VNextFramework.Auditing.AspNetCore
{
    public class AspNetCoreAuditLogContributor: AuditLogContributor
    {
        public ILogger<AspNetCoreAuditLogContributor> Logger { get; set; }

        public AspNetCoreAuditLogContributor()
        {
            Logger = NullLogger<AspNetCoreAuditLogContributor>.Instance;
        }

        public override void PreContribute(AuditLogContributionContext context)
        {
            var httpContext = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            if (httpContext == null)
            {
                return;
            }

            if (context.AuditInfo.HttpMethod == null)
            {
                context.AuditInfo.HttpMethod = httpContext.Request.Method;
            }

            if (context.AuditInfo.Url == null)
            {
                context.AuditInfo.Url = BuildUrl(httpContext);
            }

            var httpContextAccessor = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
            if (context.AuditInfo.ClientIpAddress == null)
            {
                context.AuditInfo.ClientIpAddress = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            }

            if (context.AuditInfo.BrowserInfo == null)
            {
                context.AuditInfo.BrowserInfo = httpContextAccessor.HttpContext?.Request?.Headers?["User-Agent"];
            }

        }

        public override void PostContribute(AuditLogContributionContext context)
        {
            var httpContext = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            if (httpContext == null)
            {
                return;
            }

            if (context.AuditInfo.HttpStatusCode == null)
            {
                context.AuditInfo.HttpStatusCode = httpContext.Response.StatusCode;
            }
        }

        protected virtual string BuildUrl(HttpContext httpContext)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = httpContext.Request.Scheme,
                Host = httpContext.Request.Host.Host,
                Path = httpContext.Request.Path.ToString(),
                Query = httpContext.Request.QueryString.ToString()
            };

            return uriBuilder.Uri.AbsolutePath;
        }
    }
}
