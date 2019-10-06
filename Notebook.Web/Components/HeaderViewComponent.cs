using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notebook.Business.Managers.Abstract;
using Notebook.Entities.Entities;
using Notebook.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Notebook.Web.Components
{
    public class HeaderViewComponent : ViewComponent
    {
        private IUserManager _userManager;
        private ISettingsManager _settingsManager;
        public HeaderViewComponent(IUserManager userManager, ISettingsManager settingsManager)
        {
            _userManager = userManager;
            _settingsManager = settingsManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new HeaderModel();

            model.User = HttpContext.Session.GetSession<User>("User");

            if (model.User == null)
            {
                model.User = _userManager.Cookie(HttpContext.Request.Cookies.GetCookies("Notebook"));

                if (model.User != null)
                {
                    HttpContext.Session.SetSession("User", model.User);
                }
            }

            model.Settings = await _settingsManager.Table().FirstOrDefaultAsync();

            return View(model);
        }
    }
}
