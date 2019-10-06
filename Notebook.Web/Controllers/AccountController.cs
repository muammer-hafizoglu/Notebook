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
using Notebook.Business.Tools.Mail;
using Notebook.Business.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Notebook.Web.Controllers
{
    [TypeFilter(typeof(ExceptionFilterAttribute))]
    public class AccountController : Controller
    {
        private IUserManager _userManager;
        private ISettingsManager _settingsManager;
        private IMailExtension _mailExtension;
        public AccountController(IUserManager userManager, ISettingsManager settingsManager, IMailExtension mailExtension)
        {
            _userManager = userManager;
            _settingsManager = settingsManager;
            _mailExtension = mailExtension;
        }

        [Route("~/login")]
        public IActionResult Login(string ReturnUrl = "")
        {
            var user = HttpContext.Session.GetSession<User>("User");

            if (user != null)
                return RedirectToAction("Index", "Home");

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
        public async Task<IActionResult> Register(User model, string PasswordConfirm)
        {
            if (!string.IsNullOrEmpty(model.Password) && model.Password == PasswordConfirm)
            {
                var settings = _settingsManager.Table().FirstOrDefault();
                
                _userManager.Add(model);

                var result = false;

                if (settings.MembershipEmailControl)
                {
                    model.Approve = false;
                    result = await _mailExtension.SendMail(new MailInfoModel
                    {
                        MailTo = model.Email,
                        Subject = "Mynotebook | Account activation",
                        Message = $"<a href='{settings.WebAddress}/account-activation?Username={model.Username}&Code={model.Email.SHA256Encrypt()}'>Activate</a>"
                    });
                }
                else
                    model.Approve = true;

                TempData["message"] = HelperMethods.ObjectConvertJson(new TempDataModel { type = "success", message = 
                    result ? "click on the link sent to your e-mail address to complete the membership process" : "Registration successful" });

                return View("Login");
            }

            TempData["message"] = HelperMethods.ObjectConvertJson(new TempDataModel { type = "error", message = "Passwords do not match" });
            return View();
        }

        [Route("~/account-activation")]
        public IActionResult Activation(string Username = "",string Code = "")
        {
            var user = _userManager.getOne(a => a.Username == Username);
            if (user != null)
            {
                if (user.Email.SHA256Encrypt() == Code)
                {
                    user.Approve = true;
                    _userManager.Update(user);

                    TempData["message"] = HelperMethods.ObjectConvertJson(new TempDataModel { type = "success", message = "Activation successful" });
                    return Redirect("/login");
                }
            }

            TempData["message"] = HelperMethods.ObjectConvertJson(new TempDataModel { type = "error", message = "Error: User not found" });
            return Redirect("/register");
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

            TempData["message"] = HelperMethods.ObjectConvertJson(new TempDataModel { type = "warning", message = "Not user found" });
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

            TempData["message"] = HelperMethods.ObjectConvertJson(new TempDataModel { type = "error", message = "Error" });
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
