using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Notebook.Core.CrossCuttingConcerns.Logging;
using Notebook.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notebook.Web.Filters
{
    public class ExceptionFilterAttribute : ActionFilterAttribute, IExceptionFilter
    {
        //private readonly ILoggerService _loggerService;
        //public ExceptionFilterAttribute(ILoggerService loggerService)
        //{
        //    _loggerService = loggerService;
        //}
        public void OnException(ExceptionContext context)
        {
            context.HttpContext.Session.SetSession("Exception", new ErrorModel { Message = context.Exception.Message, Source = context.Exception.Source, StackTrace = context.Exception.StackTrace });

            context.Result = new RedirectResult("/save-error");
            //context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            //{
            //    action = "SaveError",
            //    controller = "Home"
            //}));
        }
    }
}
