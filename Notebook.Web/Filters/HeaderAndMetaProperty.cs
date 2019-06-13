using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notebook.Web.Filters
{
    public class HeaderAndMetaProperty : ActionFilterAttribute
    {
        private string _description;
        public HeaderAndMetaProperty(string description = "")
        {
            _description = description;
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            context.HttpContext.Response.Headers.Add("description", new string[] { _description });
            base.OnResultExecuted(context);
        }
    }
}
