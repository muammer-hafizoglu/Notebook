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
using Notebook.Web.Models.Datatable;

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
        public FolderController(IStringLocalizer<FolderController> localizer,IFolderManager folderManager, IGroupManager groupManager, INoteManager noteManager)
        {
            _localizer = localizer;
            _folderManager = folderManager;
            _groupManager = groupManager;
            _noteManager = noteManager;
        }

        #region CRUD

        [Route("~/{ID}/folder-detail")]
        public IActionResult Notes(Parameters parameters)
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            FolderDetailModel model = new FolderDetailModel();

            model.Folder = _folderManager.GetFolderInfo(parameters.ID, _user?.ID);
            model.Navigation = new NavigationModel { List = "Notes", ID = parameters.ID };
            model.Data = NoteList(
                _noteManager.Table()
                    .Where(a => a.Folder.ID == model.Folder.ID)
                    .Include(a => a.Group)
                    .Include(a => a.Users)
                        .ThenInclude(b => b.User)
                    .OrderByDescending(a => a.CreateDate),
                parameters,
                $"/{parameters.ID}/group-detail");

            return View(model);
        }

        private ObjectListModel NoteList(IQueryable<Note> query, Parameters parameters, string url)
        {
            ObjectListModel result = new ObjectListModel();
            result.Url = url;

            if (!string.IsNullOrEmpty(parameters.Search))
            {
                url += url.Contains("?") ? "&" : "?";

                switch (parameters.Filter)
                {
                    case "ID":
                        {
                            query = query.Where(a => a.ID == parameters.Search) as IOrderedQueryable<Note>;
                            break;
                        }
                    case "Title":
                        {
                            query = query.Where(a => a.Title.Contains(parameters.Search)) as IOrderedQueryable<Note>;
                            break;
                        }
                }

                url += "Filter=" + parameters.Filter + "&Search=" + parameters.Search;
            }

            result.TotalData = query.Count();
            result.ShowInPage = int.TryParse(parameters.Show, out int _show) ? _show : 30;
            result.ActivePage = int.TryParse(parameters.Page, out int _page) ? _page : 1;
            result.TotalPage = (int)Math.Ceiling((double)result.TotalData / result.ShowInPage);
            result.Pagination = Helper.Pagination(url, result.ActivePage, result.TotalPage);
            result.Datalist = query.Skip((result.ActivePage - 1) * result.ShowInPage).Take(result.ShowInPage).ToList();

            return result;
        }

        [HttpGet]
        [Route("~/add-folder")]
        public IActionResult Add(string GroupID = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            var _group = _groupManager.getOne(a => a.ID == GroupID);
            if (_group != null)
                return PartialView("Form", new Folder { Group = _group });
            else
                TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "error", message = _localizer["Group not found"] });

            return Redirect(TempData["BeforeUrl"].ToString());
        }

        [HttpPost]
        [Route("~/addFolder")]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Folder _folder)
        {
            var _user = HttpContext.Session.GetSession<User>("User");
         
            _folderManager.Add(_folder, _user.ID);
         
            TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "success", message = _localizer["Transaction successful"] });

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
                TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "error", message = _localizer["Folder not found"] });

            return Redirect(TempData["BeforeUrl"].ToString());
        }

        [HttpPost]
        [Route("~/editFolder")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Folder _folder)
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            _folderManager.Update(_folder, _user.ID);

            TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "success", message = _localizer["Transaction successful"] });

            return Redirect(TempData["BeforeUrl"].ToString());
        }

        [HttpGet]
        [Route("~/{ID?}/delete-folder")]
        public JsonResult Delete(string ID = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            _folderManager.Delete(ID, _user.ID);

            TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "success", message = _localizer["Transaction successful"] });

            return Json("");
        }

        #endregion
    }
}
