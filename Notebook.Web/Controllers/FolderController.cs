using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Notebook.Web.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Notebook.Business.Managers.Abstract;
using Notebook.Entities.Entities;
using Notebook.Entities.Enums;
using Notebook.Web.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Notebook.Web.Controllers
{
    [TypeFilter(typeof(ExceptionFilterAttribute))]
    public class FolderController : Controller
    {

        private IStringLocalizer<FolderController> _localizer;
        private IFolderManager _folderManager;
        private IGroupManager _groupManager;
        private INoteManager _noteManager;
        private IEventManager _eventManager;
        public FolderController(IStringLocalizer<FolderController> localizer,IFolderManager folderManager, IGroupManager groupManager, 
            INoteManager noteManager, IEventManager eventManager)
        {
            _localizer = localizer;
            _folderManager = folderManager;
            _groupManager = groupManager;
            _noteManager = noteManager;
            _eventManager = eventManager;
        }

        #region List

        private FolderDetailModel GetFolderDetailModel(Parameters parameters)
        {
            var _user = HttpContext.Session.GetSession<User>("User");
            var _folder = _folderManager.GetFolderInfo(parameters.ID, _user?.ID);

            if (_folder != null)
            {
                FolderDetailModel model = new FolderDetailModel();
                model.Folder = _folder;

                return model;
            }

            return null;
        }

        [Route("~/{ID}/folder-detail")]
        public IActionResult Notes(Parameters parameters)
        {
            var model = GetFolderDetailModel(parameters);
            if (model != null)
            {
                model.Navigation = new NavigationModel { List = "Notes", ID = parameters.ID };
                model.Data = DataListOperations.List(
                    _noteManager.Table()
                        .Where(a => a.Folder.ID == model.Folder.ID)
                        .Include(a => a.Group)
                        .Include(a => a.Users)
                            .ThenInclude(b => b.User)
                        .OrderByDescending(a => a.CreateDate),
                    parameters,
                    $"/{parameters.ID}/group-detail");

                model.Data.Filters.AddRange(new String[] { "Title", "Content" });
            }

            return View(model);
        }

        #endregion

        #region CRUD

        [HttpGet]
        [Route("~/add-folder")]
        public IActionResult Add(string GroupID = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            var _group = _groupManager.getOne(a => a.ID == GroupID);
            if (_group != null)
                return PartialView("Form", new Folder { Group = _group });
            else
                TempData["message"] = HelperMethods.ObjectConvertJson(new TempDataModel { type = "error", message = _localizer["Group not found"] });

            return Redirect(TempData["BeforeUrl"].ToString());
        }

        [HttpPost]
        [Route("~/addFolder")]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Folder _folder, string View = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");
         
            _folderManager.Add(_folder, _user.ID);
            _eventManager.Add(new Event
            {
                User = _user,
                View = View == "on" ? true:false,
                Url = $"{_folder.ID}/folder-detail",
                ProductID = _folder.ID,
                ProductName = _folder.Name,
                Type = Product.Folder,
                Explation = "New folder added"
            });

            TempData["message"] = HelperMethods.ObjectConvertJson(new TempDataModel { type = "success", message = _localizer["Transaction successful"] });

            return Redirect(TempData["BeforeUrl"].ToString());
        }

        [HttpGet]
        [Route("~/{ID}/edit-folder")]
        public IActionResult Edit(string ID = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            var _folder = _folderManager.getMany(a => a.ID == ID).Include(a => a.Group).FirstOrDefault();
            if (_folder != null)
                return PartialView("Form", _folder);
            else
                TempData["message"] = HelperMethods.ObjectConvertJson(new TempDataModel { type = "error", message = _localizer["Folder not found"] });

            return Redirect(TempData["BeforeUrl"].ToString());
        }

        [HttpPost]
        [Route("~/editFolder")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Folder _folder)
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            _folderManager.Update(_folder, _user.ID);

            TempData["message"] = HelperMethods.ObjectConvertJson(new TempDataModel { type = "success", message = _localizer["Transaction successful"] });

            return Redirect(TempData["BeforeUrl"].ToString());
        }

        [HttpGet]
        [Route("~/{ID?}/delete-folder")]
        public JsonResult Delete(string ID = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            _folderManager.Delete(ID, _user.ID);

            TempData["message"] = HelperMethods.ObjectConvertJson(new TempDataModel { type = "success", message = _localizer["Transaction successful"] });

            return Json("");
        }

        #endregion
    }
}
