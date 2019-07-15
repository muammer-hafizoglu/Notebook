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
            //if (user != null)
            //{
            //    user = await _userManager.getMany(a => a.ID == user.ID).Include(a => a.Role).FirstOrDefaultAsync();

            //    user.Role = user.Role ?? new Role(); 
            //}

            return View(user);
        }
    }
}
