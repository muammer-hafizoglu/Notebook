using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Notebook.Entities.Entities;

namespace Notebook.Web.Filters
{
    public class AccountFilterAttribute : ActionFilterAttribute
    {
        private string _role;
        public AccountFilterAttribute(string role = "")
        {
            _role = role;
        }
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.Session.GetSession<User>("User");
            if (user == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    action = "Login",
                    controller = "Account"
                }));
            }
            //else if(!string.IsNullOrEmpty(_role))
            //{
            //    if (_role.Contains(","))
            //    {
            //        string[] ops = _role.Split(",");
            //        foreach (var o in ops)
            //        {
            //            if (!user.Roles.Contains(o))
            //            {
            //                ReturnError(context);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (!user.Roles.Contains(_role))
            //        {
            //            ReturnError(context);
            //        }
            //    }
            //}
        }

        private void ReturnError(ActionExecutingContext context)
        {
            context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            {
                action = "PageNotFound",
                controller = "Home"
            }));
        }
    }
}
