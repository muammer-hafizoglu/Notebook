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
        public string _lock = "<i class='fa fa-lock'></i> ";
        public string _folderIcon = "<i class='fa fa-folder-open-o'></i> ";
        public string _noteIcon = "<i class='fa fa-newspaper-o'></i> ";
        public string _userIcon = "<i class='fa fa-user'></i> ";

        private IStringLocalizer<GroupController> _localizer;
        private IGroupManager _groupManager;
        private IUserManager _userManager;
        private IUserGroupManager _userGroupManager;
        private IGroupFolderManager _groupFolderManager;
        private IGroupNoteManager _groupNoteManager;
        public GroupController(IStringLocalizer<GroupController> localizer,IGroupManager groupManager,IUserManager userManager, IUserGroupManager userGroupManager,
            IGroupFolderManager groupFolderManager, IGroupNoteManager groupNoteManager)
        {
            _localizer = localizer;
            _groupManager = groupManager;
            _userManager = userManager;
            _userGroupManager = userGroupManager;
            _groupFolderManager = groupFolderManager;
            _groupNoteManager = groupNoteManager;
        }

        #region List

        [HttpPost]
        [Route("~/groups")]
        public JsonResult List(DatatableParameters parameters, string userId)
        {
            var sqlQuery = _userGroupManager.Table()
                .Include(a => a.Group)
                .Where(a=>a.UserID == userId)
                .OrderByDescending(a=>a.Group.CreateDate) as IQueryable<UserGroup>;

            var result = new DatatableResult() { draw = parameters.draw, recordsTotal = sqlQuery.Count() };

            string search = Request.Form["search[value]"].ToString();
            if (!string.IsNullOrEmpty(search))
            {
                sqlQuery = sqlQuery.Where(a => a.Group.Name.Contains(search)) as IQueryable<UserGroup>;
            }

            result.recordsFiltered = sqlQuery.Count();

            result.data = sqlQuery.Skip(parameters.start).Take(parameters.length).Select(_ug =>
                      new GroupModel
                      {
                          name = string.Format("<a href='/{0}/group/folders'>{1}  {2}</a>", _ug.Group.ID, _ug.Group.Name, (_ug.Group.Visible == Visible.Private ? _lock : "")),
                          info = string.Format("{0}: {2} & {3}: {5}", _folderIcon, _localizer["Folder"], _ug.Group.Folders.Count(), _noteIcon, _localizer["Note"],_ug.Group.Notes.Count()),
                          state = string.Format("{0}", (_ug.MemberType == Member.Owner ? _localizer["Owner"] : _localizer["Member"]))
                      }).ToList();

            return Json(result);
        }

        #endregion

        #region CRUD

        [Route("~/{id?}/group/{list?}")]
        public IActionResult Detail(string id = "", string list = "")
        {
            GroupDetailModel detail = null;

            var _group = _groupManager.getMany(a => a.ID == id).Include(a => a.Owner).FirstOrDefault();
            if (_group != null)
            {
                detail = new GroupDetailModel();
                detail.ID = _group.ID;
                detail.Name = _group.Name;
                detail.Explanation = _group.Explanation;
                detail.CreateDate = _group.CreateDate;
                detail.Visible = _group.Visible;
                detail.OwnerID = _group.OwnerID;
                detail.OwnerName = _group.Owner.Name;
                detail.FolderCount = _groupFolderManager.getMany(a => a.GroupID == _group.ID).Count();
                detail.NoteCount = _groupNoteManager.getMany(a => a.GroupID == _group.ID).Count();
                detail.UserCount = _userGroupManager.getMany(a => a.GroupID == _group.ID).Count();
                detail.List = list;
            }
            
            return View(detail);
        }

        [HttpGet]
        [Route("~/add-group")]
        [Route("~/{id?}/edit-group")]
        public IActionResult Form(string id = "")
        {
            var _group = _groupManager.getOne(a => a.ID == id && a.OwnerID == HttpContext.Session.GetSession<User>("User").ID);

            return PartialView(_group);
        }

        [HttpPost]
        [Route("~/addGroup")]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Group _group)
        {
            var _user = HttpContext.Session.GetSession<User>("User");
            _group.Owner = _user;

            _groupManager.Add(_group);
            _groupManager.Save();

            // User - Group
            _userGroupManager.Add(new UserGroup { Group = _group, User = _user, CreateDate = DateTime.Now });
            _userGroupManager.Save();

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
            var _group = _groupManager.getOne(a => a.ID == id && a.OwnerID == _user.ID);
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
