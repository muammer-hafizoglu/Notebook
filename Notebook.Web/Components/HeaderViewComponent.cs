using Microsoft.AspNetCore.Mvc;
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
            if (user != null)
            {
                user = await _userManager.getOneAsync(a => a.ID == user.ID);
            }

            return View(user);
        }
    }
}
