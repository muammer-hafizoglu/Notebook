using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Notebook.Business.Managers.Abstract;
using Notebook.Entities.Entities;
using Notebook.Web.Filters;
using Notebook.Web.Models;
using System;
using System.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Notebook.Web.Controllers
{
    [TypeFilter(typeof(ExceptionFilterAttribute))]
    public class AccountController : Controller
    {
        private IStringLocalizer<AccountController> _localizer;
        private IUserManager _userManager;
        public AccountController(IStringLocalizer<AccountController> localizer,IUserManager userManager)
        {
            _localizer = localizer;
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            var _email = HttpContext.Request.Cookies.GetCookies("Notebook");
            if (!string.IsNullOrEmpty(_email))
            {
                var _user = _userManager.getOne(a => a.Email == _email);
                if (_user != null)
                {
                    _user.LastActiveDate = DateTime.Now;
                    _userManager.Update(_user);

                    _user.Avatar = _user.Avatar ?? "/notebook/images/avatar.png";
                    HttpContext.Session.SetSession("User", _user);

                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User user, string remember = "off")
        {
            var _user = _userManager.getOne(a => (a.Username == user.Username || a.Email == user.Username) && a.Password == user.Password.SHA256Encrypt());
            if (_user != null)
            {
                if (_user.Approve)
                {
                    if (remember == "on")
                    {
                        HttpContext.Response.Cookies.SetCookies("Notebook", _user.Email);
                    }

                    _user.LastActiveDate = DateTime.Now;
                    _userManager.Update(_user);

                    _user.Avatar = _user.Avatar ?? "/notebook/images/avatar.png";
                    HttpContext.Session.SetSession("User", _user);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["error"] = "Your account is not active";
                }
            }
            else
            {
                TempData["error"] = "Username or password is wrong";
            }

            return View();
        }

        public IActionResult Register()
        {
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User model, string PasswordConfirm)
        {
            if (!string.IsNullOrEmpty(model.Password) && model.Password == PasswordConfirm) 
            {
                // Email onayı ve avatar ayarı yapılacak
                model.Approve = true;

                _userManager.Add(model);
                _userManager.Save();

                TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "success", message = _localizer["Registration successful"] });
                return View("Login");
            }

            TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "error", message = _localizer["Passwords do not match"] });
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.SetCookies("Notebook", "", -5000);
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}
