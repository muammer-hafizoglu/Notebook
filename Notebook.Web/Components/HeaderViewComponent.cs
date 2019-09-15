using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notebook.Business.Managers.Abstract;
using Notebook.Entities.Entities;
using System.Threading.Tasks;

namespace Notebook.Web.Components
{
    public class HeaderViewComponent : ViewComponent
    {
        private IUserManager _userManager;
        public HeaderViewComponent(IUserManager userManager)
        {
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = HttpContext.Session.GetSession<User>("User");

            if (user == null)
            {
                var _user = await _userManager.CookieAsync(HttpContext.Request.Cookies.GetCookies("Notebook"));

                if (_user != null)
                {
                    HttpContext.Session.SetSession("User", _user);

                    user = _user;
                }
            }

            return View(user);
        }
    }
}
