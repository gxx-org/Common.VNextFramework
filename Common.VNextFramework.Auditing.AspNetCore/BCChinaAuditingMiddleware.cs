using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Common.VNextFramework.Auditing.AspNetCore
{
    public class BCChinaAuditingMiddleware : IMiddleware
    {
        private readonly IAuditingManager _auditingManager;
        protected AuditingOptions AuditingOptions { get; }
        protected AspNetCoreAuditingOptions AspNetCoreAuditingOptions { get; }
        public BCChinaAuditingMiddleware(
            IAuditingManager auditingManager,
            IOptions<AuditingOptions> auditingOptions, 
            IOptions<AspNetCoreAuditingOptions> aspNetCoreAuditingOptions)
        {
            _auditingManager = auditingManager;
            AspNetCoreAuditingOptions = aspNetCoreAuditingOptions.Value;
            AuditingOptions = auditingOptions.Value;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!AuditingOptions.IsEnabled || IsIgnoredUrl(context))
            {
                await next(context);
                return;
            }

            var hasError = false;
            using (var saveHandle = _auditingManager.BeginScope())
            {

                try
                {
                    await next(context);

                    if (_auditingManager.Current.Log.Exceptions.Any())
                    {
                        hasError = true;
                    }
                }
                catch (Exception ex)
                {
                    hasError = true;

                    if (!_auditingManager.Current.Log.Exceptions.Contains(ex))
                    {
                        _auditingManager.Current.Log.Exceptions.Add(ex);
                    }

                    throw;
                }
                finally
                {
                    if (ShouldWriteAuditLog(context, hasError))
                    {
                        await saveHandle.SaveAsync();
                    }
                }
            }
        }

        private bool ShouldWriteAuditLog(HttpContext httpContext, bool hasError)
        {
            if (AuditingOptions.AlwaysLogOnException && hasError)
            {
                return true;
            }

            //TODO: create ICurrentUser interface and has IsAuthenticated property 
            if (!AuditingOptions.IsEnabledForAnonymousUsers && httpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                return false;
            }

            if (!AuditingOptions.IsEnabledForGetRequests &&
                string.Equals(httpContext.Request.Method, HttpMethods.Get, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        private bool IsIgnoredUrl(HttpContext context)
        {
            return context.Request.Path.Value != null &&
                   AspNetCoreAuditingOptions.IgnoredUrls.Any(x => context.Request.Path.Value.StartsWith(x));
        }
    }
}
