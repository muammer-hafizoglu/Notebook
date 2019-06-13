using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Notebook.Business.Managers.Abstract;
using Notebook.Entities.Entities;
using System.Threading.Tasks;

namespace Notebook.Web.Components
{
    public class AsideViewComponent : ViewComponent
    {
        private IUserManager userManager;
        private readonly IStringLocalizer<AsideViewComponent> localizer;
        public AsideViewComponent(IUserManager _userManager, IStringLocalizer<AsideViewComponent> _localizer)
        {
            userManager = _userManager;
            localizer = _localizer;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewData["localizer"] = localizer;

            var user = HttpContext.Session.GetSession<User>("User");

            var _user = await userManager.getMany(a => a.ID == user.ID).FirstOrDefaultAsync();
            if (_user != null)
            {
                user = _user;
            }

            return View(user);
        }
    }
}
