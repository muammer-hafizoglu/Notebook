using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notebook.Business.Managers.Abstract;
using Notebook.Business.Models;
using Notebook.Business.Tools.Logging;
using Notebook.Business.Tools.Mail;
using Notebook.Core.Aspects.SimpleProxy.Logging;
using Notebook.Core.CrossCuttingConcerns.Logging;
using Notebook.Entities.Entities;
using Notebook.Web.Filters;
using Notebook.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notebook.Web.Controllers
{
    //[TypeFilter(typeof(AccountFilterAttribute))]
    [TypeFilter(typeof(ExceptionFilterAttribute))]
    public class HomeController : Controller
    {
        private IUserManager _userManager;
        private ISettingsManager _settingsManager;
        private IEventManager _eventManager;
        private IMailExtension _mailExtension;
        public HomeController(IUserManager userManager, ISettingsManager settingsManager, IMailExtension mailExtension, IEventManager eventManager)
        {
            _userManager = userManager;
            _settingsManager = settingsManager;
            _mailExtension = mailExtension;
            _eventManager = eventManager;
        }
        
        public IActionResult Index(Parameters parameters)
        {
            var user = HttpContext.Session.GetSession<User>("User");

            var model = new IndexPageModel();

            if (user != null)
            {
                user = _userManager.Table().Where(a => a.ID == user.ID).Include(a => a.Following).FirstOrDefault();

                model.Data = DataListOperations.List(
                    _eventManager.getMany(a => user.Following.Any(b => b.FollowingID == a.User.ID) && a.View == true)
                        .Include(a => a.User)
                        .OrderByDescending(a => a.CreateDate),
                    parameters,
                    $"/");
            }
            else
            {
                model.Explanation = _settingsManager.Table().FirstOrDefault()?.Introduction;
            }

            return View(model);
        }

        [Route("~/error-page")]
        public IActionResult Error()
        {
            return View();
        }

        [Route("~/save-error")]
        public IActionResult SaveError()
        {
            var exception = HttpContext.Session.GetSession<ErrorModel>("Exception");
            if (exception != null)
            {
                // TODO: Sistem hatalarını kaydet

                HttpContext.Session.Remove("Exception");

                TempData["message"] = HelperMethods.ObjectConvertJson(new TempDataModel { type = "error", message = exception.Message });
            }

            return Redirect(TempData["BeforeUrl"] as string);
        }
    }
}
