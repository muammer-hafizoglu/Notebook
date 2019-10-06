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
        private IEventManager _eventManager;
        public GroupController(IStringLocalizer<GroupController> localizer,IGroupManager groupManager,IUserManager userManager, IUserGroupManager userGroupManager, 
            IFolderManager folderManager, INoteManager noteManager, IEventManager eventManager)
        {
            _localizer = localizer;
            _groupManager = groupManager;
            _userManager = userManager;
            _userGroupManager = userGroupManager;
            _folderManager = folderManager;
            _noteManager = noteManager;
            _eventManager = eventManager;
        }

        #region List

        private GroupDetailModel GetGroupDetailModel(Parameters parameters)
        {
            var _user = HttpContext.Session.GetSession<User>("User");
            var _group = _groupManager.GetGroupInfo(parameters.ID, _user?.ID);

            if (_group != null)
            {
                GroupDetailModel model = new GroupDetailModel();
                model.Group = _group;

                return model;
            }

            return null;
        }

        [Route("~/{ID}/group-detail")]
        public IActionResult Folders(Parameters parameters)
        {
            var model = GetGroupDetailModel(parameters);
            if (model != null)
            {
                model.Navigation = new NavigationModel { List = "Folders", ID = parameters.ID };
                model.Data = DataListOperations.List(
                    _folderManager.Table()
                        .Where(a => a.Group.ID == model.Group.ID)
                        .Include(a => a.Group)
                        .Include(a => a.Notes)
                        .OrderByDescending(a => a.CreateDate),
                    parameters,
                    $"/{parameters.ID}/group-detail");

                model.Data.Filters.AddRange(new String[] { "Name" });
            }

            return View(model);
        }

        [Route("~/{ID}/group-detail/notes")]
        public IActionResult Notes(Parameters parameters)
        {
            var model = GetGroupDetailModel(parameters);
            if (model != null)
            {
                model.Navigation = new NavigationModel { List = "Notes", ID = parameters.ID };
                model.Data = DataListOperations.List(
                    _noteManager.Table()
                        .Where(a => a.Group.ID == model.Group.ID)
                        .Include(a => a.Group)
                        .Include(a => a.Users)
                            .ThenInclude(b => b.User)
                        .OrderByDescending(a => a.CreateDate),
                    parameters,
                    $"/{parameters.ID}/group-detail/notes");

                model.Data.Filters.AddRange(new String[] { "Title", "Content" });
            }

            return View(model);
        }

        [Route("~/{ID}/group-detail/members")]
        public IActionResult Members(Parameters parameters)
        {
            var model = GetGroupDetailModel(parameters);
            if (model != null)
            {
                model.Navigation = new NavigationModel { List = "Members", ID = parameters.ID };
                model.Data = MemberList(
                    _userGroupManager.Table()
                        .Where(a => a.Group.ID == model.Group.ID)
                        .Include(a => a.Group)
                        .Include(a => a.User)
                        .OrderByDescending(a => a.CreateDate),
                    parameters,
                    $"/{parameters.ID}/group-detail/members");

                model.Data.Filters.AddRange(new String[] { "Name", "Username" });
            }

            return View(model);
        }

        private ObjectListModel MemberList(IQueryable<UserGroup> query, Parameters parameters, string url)
        {
            ObjectListModel result = new ObjectListModel();
            result.Url = url;

            if (!string.IsNullOrEmpty(parameters.Search))
            {
                url += url.Contains("?") ? "&" : "?";

                query = query.Where(a => EF.Property<string>(a.User, parameters.Filter).Contains(parameters.Search)) as IOrderedQueryable<UserGroup>;

                url += "Filter=" + parameters.Filter + "&Search=" + parameters.Search;
            }

            result = DataListOperations.ListOperation(result, query, parameters, url);

            return result;
        }

        #endregion

        #region Add-Edit-Delete

        [TypeFilter(typeof(AccountFilterAttribute))]
        [HttpGet]
        [Route("~/add-group")]
        [Route("~/{ID?}/edit-group")]
        public IActionResult Form(string ID = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");
            var _userGroup = _userGroupManager
                            .getMany(a => a.GroupID == ID && a.UserID == _user.ID)
                            .Include(a => a.Group)
                            .FirstOrDefault();

            var _group = _userGroup != null ? _userGroup.Group : new Group { Visible = Visible.Public };

            return PartialView(_group);
        }

        [TypeFilter(typeof(AccountFilterAttribute))]
        [HttpPost]
        [Route("~/addGroup")]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Group _group, string View = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            _groupManager.Add(_group);
            _eventManager.Add(new Event
            {
                User = _user,
                View = View == "on" ? true:false,
                Url = $"{_group.ID}/group-detail",
                ProductID = _group.ID,
                ProductName = _group.Name,
                Type = Product.Group,
                Explation = "New group added"
            });

            _userGroupManager.Add(new UserGroup { Group = _group, User = _user, CreateDate = DateTime.Now, Status = Status.Owner });

            TempData["message"] = HelperMethods.ObjectConvertJson(new TempDataModel { type = "success", message = _localizer["Transaction successful"] });

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

                TempData["message"] = HelperMethods.ObjectConvertJson(new TempDataModel { type = "success", message = _localizer["Transaction successful"] });
            }
            else
            {
                TempData["message"] = HelperMethods.ObjectConvertJson(new TempDataModel { type = "error", message = _localizer["Group not found"] });
            }

            return Redirect(TempData["BeforeUrl"].ToString());
        }

        [HttpGet]
        [Route("~/{ID?}/delete-group")]
        [TypeFilter(typeof(AccountFilterAttribute))]
        public JsonResult Delete(string ID = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            _groupManager.Delete(ID, _user.ID);

            TempData["message"] = HelperMethods.ObjectConvertJson(new TempDataModel { type = "success", message = _localizer["Transaction successful"] });

            return Json("");
        }

        #endregion

        #region Member Operaitons

        [HttpGet]
        [Route("~/{ID?}/join-group")]
        [TypeFilter(typeof(AccountFilterAttribute))]
        public JsonResult Join(string ID = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            _userGroupManager.Join(ID, _user.ID);

            return Json("");
        }

        [HttpGet]
        [Route("~/{ID?}/exit-group")]
        [TypeFilter(typeof(AccountFilterAttribute))]
        public JsonResult Exit(string ID = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            _userGroupManager.Exit(ID, _user.ID);
            
            return Json("");
        }

        [HttpGet]
        [Route("~/{ID?}/delete-member")]
        [TypeFilter(typeof(AccountFilterAttribute))]
        public JsonResult DeleteMember(string ID = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            _userGroupManager.Delete(ID, _user.ID);

            return Json("");
        }

        [TypeFilter(typeof(AccountFilterAttribute))]
        [HttpGet]
        [Route("~/{ID?}/edit-member")]
        public IActionResult EditMember(string ID = "")
        {
            var member = _userGroupManager.getOne(a => a.ID == ID);

            return PartialView(member);
        }

        [TypeFilter(typeof(AccountFilterAttribute))]
        [HttpPost]
        [Route("~/editMember")]
        [ValidateAntiForgeryToken]
        public IActionResult EditMemberPost(UserGroup member)
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            var _member = _userGroupManager.getOne(a => a.ID == member.ID);
            _member.Status = member.Status;

            _userGroupManager.Update(_member);

            return Redirect(TempData["BeforeUrl"].ToString());
        }
        #endregion
    }
}
