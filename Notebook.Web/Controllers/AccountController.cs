using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Notebook.Business.Managers.Abstract;
using Notebook.Entities.Entities;
using Notebook.Web.Filters;
using Notebook.Web.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Notebook.Web.Controllers
{
    [TypeFilter(typeof(ExceptionFilterAttribute))]
    public class AccountController : Controller
    {
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly IUserManager _userManager;
        public AccountController(IStringLocalizer<AccountController> localizer, IUserManager userManager)
        {
            _localizer = localizer;
            _userManager = userManager;
        }

        [Route("~/login")]
        public IActionResult Login()
        {
            var user = _userManager.Cookie(HttpContext.Request.Cookies.GetCookies("Notebook"));
            if (user != null)
            {
                HttpContext.Session.SetSession("User", user);

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [Route("~/login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User user, string remember = "off")
        {
            var _user = _userManager.Login(user);

            HttpContext.Session.SetSession("User", _user);

            if (remember == "on")
            {
                HttpContext.Response.Cookies.SetCookies("Notebook", _user.Email);
            }

            return RedirectToAction("Index", "Home");
        }

        [Route("~/register")]
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
                // TODO: Email onayı gerekliliğini kontrol et ve avatar ayarı yapılacak
                model.Approve = true;

                _userManager.Add(model);

                TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "success", message = _localizer["Registration successful"] });
                return View("Login");
            }

            TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "error", message = _localizer["Passwords do not match"] });
            return View();
        }

        #region Social Media Login-Register

        public IActionResult SocialMediaLogin(String provider)
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/Account/SocialMediaLoginPost" }, provider);
        }

        public IActionResult SocialMediaLoginPost()
        {
            if (User.Identity.IsAuthenticated)
            {
                var _user = _userManager.Cookie(User.Claims.Where(a => a.Type.Contains("emailaddress")).FirstOrDefault().Value);

                if (_user != null)
                {
                    HttpContext.Session.SetSession("User", _user);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return SocialMediaRegisterPost();
                }
            }

            TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "warning", message = _localizer["Not user found"] });
            return RedirectToAction("Login");
        }

        public IActionResult SocialMediaRegister(String provider)
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/Account/SocialMediaRegisterPost" }, provider);
        }

        public IActionResult SocialMediaRegisterPost()
        {
            if (User.Identity.IsAuthenticated)
            {
                User user = new User
                {
                    Email = User.Claims.Where(a => a.Type.Contains("emailaddress")).FirstOrDefault().Value,
                    Name = User.Claims.Where(a => a.Type.Contains("claims/name")).LastOrDefault().Value,
                    Approve = true,
                    Password = Guid.NewGuid().ToString().Substring(0, 8),
                    CreateDate = DateTime.Now
                };

                _userManager.Add(user);

                return SocialMediaLoginPost();
            }

            TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "error", message = _localizer["Error"] });
            return RedirectToAction("Login");
        }

        #endregion

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [Route("~/logout")]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.SetCookies("Notebook", "", -5000);
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}
