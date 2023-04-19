using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.VNextFramework.Auditing.AspNetCore
{


    public class AuditActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!ShouldSaveAudit(context, out var auditLog, out var auditLogAction))
            {
                await next();
                return;
            }

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var result = await next();

                if (result.Exception != null && !result.ExceptionHandled)
                {
                    auditLog.Exceptions.Add(result.Exception);
                }
            }
            catch (Exception ex)
            {
                auditLog.Exceptions.Add(ex);
                throw;
            }
            finally
            {
                stopwatch.Stop();
                auditLogAction.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
                auditLog.Actions.Add(auditLogAction);
            }
        }

        private bool ShouldSaveAudit(ActionExecutingContext context, out AuditLogInfo auditLog, out AuditLogActionInfo auditLogAction)
        {
            auditLog = null;
            auditLogAction = null;

            var options = context.HttpContext.RequestServices.GetRequiredService<IOptions<AuditingOptions>>().Value;
            if (!options.IsEnabled)
            {
                return false;
            }

            if (!(context.ActionDescriptor is ControllerActionDescriptor))
            {
                return false;
            }

            var auditLogScope = context.HttpContext.RequestServices.GetRequiredService<IAuditingManager>().Current;
            if (auditLogScope == null)
            {
                return false;
            }

            var auditingHelper = context.HttpContext.RequestServices.GetRequiredService<IAuditingHelper>();
            if (!auditingHelper.ShouldSaveAudit(((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo, true))
            {
                return false;
            }

            auditLog = auditLogScope.Log;
            var auditingFactory = context.HttpContext.RequestServices.GetRequiredService<IAuditingFactory>();
            auditLogAction = auditingFactory.CreateAuditLogAction(
                auditLog,
                ((ControllerActionDescriptor)context.ActionDescriptor).ControllerTypeInfo.AsType(),
                ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo,
                context.ActionArguments
            );

            return true;
        }
    }
}
