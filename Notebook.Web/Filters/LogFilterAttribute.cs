using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Notebook.Business.Tools.Logging;
using Notebook.Core.Aspects.SimpleProxy.Logging;
using Notebook.Core.CrossCuttingConcerns.Logging;
using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notebook.Web.Filters
{
    public class LogFilterAttribute : ActionFilterAttribute
    {
        private readonly ILoggerService _loggerService;
        public LogFilterAttribute(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            LogDetail log = new LogDetail
            {
                DateTime = DateTime.Now,
                FullName = context.RouteData.Values["Controller"].ToString(),
                MethodName = context.RouteData.Values["Action"].ToString(),
                Type = LogType.Info.ToString(),
                Info = context.RouteData.Values["LogInfo"] != null ? context.RouteData.Values["LogInfo"].ToString() : "",
                IPAddress = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                Arguments = context.ActionDescriptor.Parameters.Select((param, i) =>
                    new LogArgument {
                        Name = param.Name,
                        Type = param.ParameterType.Name,
                        Value = context.HttpContext.Request.Query[param.Name].ToString()
                    }).ToList()
            };

            var user = context.HttpContext.Session.GetSession<User>("User");
            log.UserName = user != null ? "Username: " + user.Username + " | Email: " + user.Email : "Visitor";

            _loggerService.Logging(log);
        }
    }
}
