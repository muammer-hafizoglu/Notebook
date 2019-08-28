using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Notebook.Business.Managers.Abstract;
using Notebook.Entities.Entities;
using Notebook.Entities.Enums;
using Notebook.Web.Filters;
using Notebook.Web.Models;
using Notebook.Web.Models.Datatable;

namespace Notebook.Web.Controllers
{
    [TypeFilter(typeof(ExceptionFilterAttribute))]
    public class GroupController : Controller
    {
        private IStringLocalizer<GroupController> _localizer;
        private IGroupManager _groupManager;
        private IUserManager _userManager;
        private IUserGroupManager _userGroupManager;
        private IFolderManager _folderManager;
        private INoteManager _noteManager;
        public GroupController(IStringLocalizer<GroupController> localizer,IGroupManager groupManager,IUserManager userManager, IUserGroupManager userGroupManager, 
            IFolderManager folderManager, INoteManager noteManager)
        {
            _localizer = localizer;
            _groupManager = groupManager;
            _userManager = userManager;
            _userGroupManager = userGroupManager;
            _folderManager = folderManager;
            _noteManager = noteManager;
        }

        #region CRUD

        [Route("~/{ID}/group-detail")]
        public IActionResult Folders(Parameters parameters)
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            GroupDetailModel model = new GroupDetailModel();

            model.Group = _groupManager.GetGroupInfo(parameters.ID, _user?.ID);
            model.Navigation = new NavigationModel { List = "Folders", ID = parameters.ID };
            model.Data = FolderList(
                _folderManager.Table()
                    .Where(a => a.Group.ID == model.Group.ID)
                    .Include(a => a.Group)
                    .OrderByDescending(a => a.CreateDate),
                parameters,
                $"/{parameters.ID}/group-detail");

            return View(model);
        }

        private ObjectListModel FolderList(IQueryable<Folder> query, Parameters parameters, string url)
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
                            query = query.Where(a => a.ID == parameters.Search) as IOrderedQueryable<Folder>;
                            break;
                        }
                    case "Name":
                        {
                            query = query.Where(a => a.Name.Contains(parameters.Search)) as IOrderedQueryable<Folder>;
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

        [Route("~/{ID}/group-detail/notes")]
        public IActionResult Notes(Parameters parameters)
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            GroupDetailModel model = new GroupDetailModel();

            model.Group = _groupManager.GetGroupInfo(parameters.ID, _user.ID);
            model.Navigation = new NavigationModel { List = "Notes", ID = parameters.ID };
            model.Data = NoteList(
                _noteManager.Table()
                    .Where(a => a.Group.ID == model.Group.ID)
                    .Include(a => a.Group)
                    .Include(a => a.Users)
                        .ThenInclude(b => b.User)
                    .OrderByDescending(a => a.CreateDate),
                parameters,
                $"/{parameters.ID}/group-detail/notes");

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

        [TypeFilter(typeof(AccountFilterAttribute))]
        [HttpGet]
        [Route("~/add-group")]
        [Route("~/{id?}/edit-group")]
        public IActionResult Form(string id = "")
        {
            var _group = _groupManager.getOne(a => a.ID == id && a.UserID == HttpContext.Session.GetSession<User>("User").ID);

            return PartialView(_group ?? new Group { Visible = Visible.Public });
        }

        [TypeFilter(typeof(AccountFilterAttribute))]
        [HttpPost]
        [Route("~/addGroup")]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Group _group)
        {
            var _user = HttpContext.Session.GetSession<User>("User");
           // _group.Users = _user;

            _groupManager.Add(_group);
            _groupManager.Save();

            // User - Group
            _userGroupManager.Add(new UserGroup { Group = _group, User = _user, CreateDate = DateTime.Now, Status = Status.Owner });

            TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "success", message = _localizer["Transaction successful"] });

            return Redirect(TempData["BeforeUrl"].ToString());
        }

        [HttpPost]
        [Route("~/editGroup")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Group _group)
        {
            if (!string.IsNullOrEmpty(_group.ID))
            {
                _groupManager.Update(_group);
                _groupManager.Save();

                TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "success", message = _localizer["Transaction successful"] });
            }
            else
            {
                TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "error", message = _localizer["Group not found"] });
            }

            return Redirect(TempData["BeforeUrl"].ToString());
        }

        [HttpGet]
        [Route("~/{id?}/delete-group")]
        public JsonResult Delete(string id = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");
            var _group = _groupManager.getOne(a => a.ID == id && a.UserID == _user.ID);
            if (_group != null)
            {
                _groupManager.Delete(_group);
                _groupManager.Save();

                TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "success", message = _localizer["Transaction successful"] });
            }
            else
            {
                TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "error", message = _localizer["Group not found"] });
                
            }

            return Json("");
        }
        #endregion
    }
}
