using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notebook.Business.Managers.Abstract;
using Notebook.Entities.Entities;
using System.Threading.Tasks;

namespace Notebook.Web.Components
{
    public class FooterViewComponent : ViewComponent
    {
        private ISettingsManager _settingsManager;
        public FooterViewComponent(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var setting = await _settingsManager.getAll().FirstOrDefaultAsync();

            return View(setting);
        }
    }
}
