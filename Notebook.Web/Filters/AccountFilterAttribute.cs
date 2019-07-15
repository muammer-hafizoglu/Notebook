using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Notebook.Entities.Entities;

namespace Notebook.Web.Filters
{
    public class AccountFilterAttribute : ActionFilterAttribute
    {
        private readonly string _permission;
        public AccountFilterAttribute(string permission = "")
        {
            _permission = permission;
        }
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.Session.GetSession<User>("User");
            if (user == null)
            {
                context.Result = new RedirectResult("/login");
            }
            else if (!string.IsNullOrEmpty(_permission))
            {
                if (user.Role != null)
                {
                    if (_permission.Contains(","))
                    {
                        string[] ops = _permission.Split(",");
                        foreach (var o in ops)
                        {
                            if (!user.Role.Permissions.Contains(o))
                            {
                                ReturnError(context, "You are not authorized for view this page");
                            }
                        }
                    }
                    else
                    {
                        if (!user.Role.Permissions.Contains(_permission))
                        {
                            ReturnError(context, "You are not authorized for view this page");
                        }
                    }
                }
                else
                {
                    ReturnError(context, "You are not authorized for view this page");
                }
            }
        }

        private void ReturnError(ActionExecutingContext context,string message = "")
        {
            var cntrl = context.Controller as Controller;
            cntrl.TempData["Error"] = message;

            context.Result = new RedirectResult("/error-page");
        }
    }
}
