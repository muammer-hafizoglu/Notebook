using Microsoft.AspNetCore.Mvc;
using Notebook.Business.Managers.Abstract;
using Notebook.Business.Tools.Logging;
using Notebook.Core.Aspects.SimpleProxy.Logging;
using Notebook.Core.CrossCuttingConcerns.Logging;
using Notebook.Entities.Entities;
using Notebook.Web.Filters;
using Notebook.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Notebook.Web.Controllers
{
    //[TypeFilter(typeof(AccountFilterAttribute))]
    [TypeFilter(typeof(ExceptionFilterAttribute))]
    public class HomeController : Controller
    {
        private readonly IUserManager _userManager;
        public HomeController(IUserManager userManager)
        {
            _userManager = userManager;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PageNotFound()
        {
            return View();
        }
        public IActionResult Error()
        {
            var exception = HttpContext.Session.GetSession<ErrorModel>("Exception");
            if (exception != null)
            {
                HttpContext.Session.Remove("Exception");
                TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "error", message = exception.Message });
            }

            return Redirect(TempData["BeforeUrl"] as string);
        }
    }
}
