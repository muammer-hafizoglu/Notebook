using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notebook.Business.Managers.Abstract;
using Notebook.Business.Models;
using Notebook.Entities.Entities;
using Notebook.Entities.Enums;
using Notebook.Web.Filters;
using Notebook.Web.Models;
using Notebook.Web.Tools.FileManager;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Notebook.Web.Controllers
{
    [TypeFilter(typeof(ExceptionFilterAttribute))]
    public class UserController : Controller
    {
        private IUserManager _userManager;
        private IUserGroupManager _userGroupManager;
        private IUserNoteManager _userNoteManager;
        private IFollowManager _followManager;
        private IEventManager _eventManager;
        private IFileManager _fileManager;
        public UserController(IUserManager userManager, IUserGroupManager userGroupManager, IUserNoteManager userNoteManager, IFollowManager followManager,
            IEventManager eventManager, IFileManager fileManager)
        {
            _userManager = userManager;
            _userGroupManager = userGroupManager;
            _userNoteManager = userNoteManager;
            _followManager = followManager;
            _eventManager = eventManager;
            _fileManager = fileManager;
        }

        #region Edit User

        [TypeFilter(typeof(AccountFilterAttribute))]
        [HttpGet]
        [Route("~/edit-profile")]
        public IActionResult Settings()
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            var model = new ProfileEditModel();

            model.UserInfo = _userManager.GetUserInfo(_user.Username, _user.ID);
            model.User = _userManager.getOne(a => a.ID == _user.ID);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("~/editProfile")]
        public IActionResult Settings(User model, string NewPassword,string Lock, IFormFile Avatar)
        {
            if (model != null && model.ID != null)
            {
                var user = _userManager.getOne(a => a.ID == model.ID);
                if (user != null)
                {
                    user.Name = model.Name;
                    user.Username = model.Username;
                    user.Email = model.Email;
                    user.Info = model.Info;
                    user.Lock = Lock != null && Lock == "on" ? true : false;

                    if (!ChangePassword(user,model.Password,NewPassword))
                    {
                        TempData["message"] = HelperMethods.ObjectConvertJson(new TempDataModel { type = "error", message = "Password error" });
                        return Redirect("/edit-profile");
                    }

                    UploadAvatar(Avatar, user);

                    _userManager.Update(user);

                    HttpContext.Session.Clear();
                    HttpContext.Session.SetSession("User",user);

                    TempData["message"] = HelperMethods.ObjectConvertJson(new TempDataModel { type = "success", message = "Transaction successful" });
                    return Redirect("/edit-profile");
                }
            }

            TempData["message"] = HelperMethods.ObjectConvertJson(new TempDataModel { type = "error", message = "User not found" });
            return Redirect("/edit-profile");
        }

        private void UploadAvatar(IFormFile avatar, User user)
        {
            if (avatar != null)
            {
                if(user.Avatar != "/notebook/images/avatar.png")
                    _fileManager.Delete(_fileManager.GetWebRootPath() + user.Avatar);

                user.Avatar = _fileManager.Add(new FileModel
                {
                    FormFile = avatar,
                    IsWebRoot = true,
                    Name = "avatar",
                    Path = "/notebook/users/" + user.Username
                });
            }
        }

        private bool ChangePassword(User user, string Password, string NewPassword)
        {
            if (!string.IsNullOrEmpty(NewPassword) && NewPassword.Length > 5)
            {
                if (Password.SHA256Encrypt() == user.Password)
                {
                    user.Password = NewPassword.SHA256Encrypt();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Follow

        [Route("~/{ID}/follow-user")]
        [HttpGet]
        public JsonResult Follow(string ID = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            _followManager.Follow(ID, _user?.Username);

            return Json("");
        }

        [Route("~/{ID}/unfollow-user")]
        [HttpGet]
        public JsonResult Unfollow(string ID = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            _followManager.Unfollow(ID, _user?.Username);

            return Json("");
        }

        [Route("~/{ID}/delete-follow")]
        [HttpGet]
        public JsonResult DeleteFollow(string ID = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            _followManager.Delete(ID, _user?.Username);

            return Json("");
        }

        [TypeFilter(typeof(AccountFilterAttribute))]
        [HttpGet]
        [Route("~/{ID?}/edit-follow")]
        public IActionResult EditFollow(string ID = "")
        {
            var follow = _followManager.getOne(a => a.ID == ID);

            return PartialView(follow);
        }

        [TypeFilter(typeof(AccountFilterAttribute))]
        [HttpPost]
        [Route("~/editFollow")]
        [ValidateAntiForgeryToken]
        public IActionResult EditFollowPost(Follow follow)
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            var _follow = _followManager.getOne(a => a.ID == follow.ID);
            _follow.Status = follow.Status;

            _followManager.Update(_follow);

            return Redirect(TempData["BeforeUrl"].ToString());
        }

        #endregion

        #region Event

        [Route("~/{ID}/show-event")]
        [HttpGet]
        public JsonResult ShowEvent(string ID = "")
        {
            _eventManager.Show(ID);

            return Json("");
        }

        [Route("~/{ID}/hide-event")]
        [HttpGet]
        public JsonResult HideEvent(string ID = "")
        {
            _eventManager.Hide(ID);

            return Json("");
        }

        [Route("~/{ID}/delete-event")]
        [HttpGet]
        public JsonResult DeleteEvent(string ID = "")
        {
            _eventManager.Delete(ID);

            return Json("");
        }

        #endregion

        #region List

        private ProfileModel GetUserDetailModel(string ID)
        {
            var _user = HttpContext.Session.GetSession<User>("User");
            var user = _userManager.GetUserInfo(ID, _user?.ID);

            if (user != null)
            {
                ProfileModel model = new ProfileModel();
                model.User = user;

                return model;
            }

            return null;
        }

        [Route("~/{ID}")]
        public IActionResult Timeline(Parameters parameters)
        {
            var model = GetUserDetailModel(parameters.ID);
            if (model != null)
            {
                model.Navigation = new NavigationModel { List = "Timeline", ID = parameters.ID };
                model.Data = DataListOperations.List(
                    _eventManager.GetEventsForTimeline(model.User.ID,model.User.Status),
                    parameters,
                    $"/{model.User.Username}");
            }

            return View(model);
        }

        [Route("~/{ID}/groups")]
        public IActionResult Groups(Parameters parameters)
        {
            var model = GetUserDetailModel(parameters.ID);
            if (model != null)
            {
                model.Navigation = new NavigationModel { List = "Groups", ID = parameters.ID };
                model.Data = GroupList(
                    _userGroupManager.Table()
                        .Where(a => a.UserID == model.User.ID)
                        .Include(a => a.User)
                        .Include(a => a.Group.Notes)
                        .Include(a => a.Group.Folders)
                        .OrderByDescending(a => a.CreateDate),
                    parameters,
                    $"/{parameters.ID}/groups");

                model.Data.Filters.AddRange(new String[] { "Name" });
            }

            return View(model);
        }

        private ObjectListModel GroupList(IQueryable<UserGroup> query, Parameters parameters, string url)
        {
            ObjectListModel result = new ObjectListModel();
            result.Url = url;

            if (!string.IsNullOrEmpty(parameters.Search))
            {
                url += url.Contains("?") ? "&" : "?";

                query = query.Where(a => EF.Property<string>(a.Group, parameters.Filter).Contains(parameters.Search)) as IOrderedQueryable<UserGroup>;

                url += "Filter=" + parameters.Filter + "&Search=" + parameters.Search;
            }

            result = DataListOperations.ListOperation(result, query, parameters, url);

            return result;
        }

        [Route("~/{ID}/notes")]
        public IActionResult Notes(Parameters parameters)
        {
            var model = GetUserDetailModel(parameters.ID);
            if (model != null)
            {
                model.Navigation = new NavigationModel { List = "Notes", ID = parameters.ID };
                model.Data = NoteList(
                    _userNoteManager.Table()
                        .Where(a => a.UserID == model.User.ID)
                        .Include(a => a.User)
                        .Include(a => a.Note.Group)
                        .Include(a => a.Note.Folder)
                        .OrderByDescending(a => a.CreateDate),
                    parameters,
                    $"/{parameters.ID}/notes");

                model.Data.Filters.AddRange(new String[]{ "Title","Content"});
            }

            return View(model);
        }

        private ObjectListModel NoteList(IQueryable<UserNote> query, Parameters parameters, string url)
        {
            ObjectListModel result = new ObjectListModel();
            result.Url = url;

            if (!string.IsNullOrEmpty(parameters.Search))
            {
                url += url.Contains("?") ? "&" : "?";

                query = query.Where(a => EF.Property<string>(a.Note, parameters.Filter).Contains(parameters.Search)) as IOrderedQueryable<UserNote>;

                url += "Filter=" + parameters.Filter + "&Search=" + parameters.Search;
            }

            result = DataListOperations.ListOperation(result, query, parameters, url);

            return result;
        }

        [Route("~/{ID}/followers")]
        public IActionResult Followers(Parameters parameters)
        {
            var model = GetUserDetailModel(parameters.ID);
            if (model != null)
            {
                model.Navigation = new NavigationModel { List = "Followers", ID = parameters.ID };
                model.Data = FollowerList(
                    _followManager.Table()
                        .Where(a => a.FollowingID == model.User.ID)
                        .Include(a => a.Follower)
                        .OrderByDescending(a => a.CreateDate),
                    parameters,
                    $"/{parameters.ID}/notes");

                model.Data.Filters.AddRange(new String[] { "Name", "Username" });
            }

            return View(model);
        }

        private ObjectListModel FollowerList(IQueryable<Follow> query, Parameters parameters, string url)
        {
            ObjectListModel result = new ObjectListModel();
            result.Url = url;

            if (!string.IsNullOrEmpty(parameters.Search))
            {
                url += url.Contains("?") ? "&" : "?";

                query = query.Where(a => EF.Property<string>(a.Follower, parameters.Filter).Contains(parameters.Search)) as IOrderedQueryable<Follow>;

                url += "Filter=" + parameters.Filter + "&Search=" + parameters.Search;
            }

            result = DataListOperations.ListOperation(result, query, parameters, url);

            return result;
        }

        [Route("~/{ID}/following")]
        public IActionResult Following(Parameters parameters)
        {
            var model = GetUserDetailModel(parameters.ID);
            if (model != null)
            {
                model.Navigation = new NavigationModel { List = "Following", ID = parameters.ID };
                model.Data = FollowingList(
                    _followManager.Table()
                        .Where(a => a.FollowerID == model.User.ID && a.Status != Status.Blocked)
                        .Include(a => a.Following)
                        .OrderByDescending(a => a.CreateDate),
                    parameters,
                    $"/{parameters.ID}/notes");

                model.Data.Filters.AddRange(new String[] { "Name", "Username" });
            }

            return View(model);
        }

        private ObjectListModel FollowingList(IQueryable<Follow> query, Parameters parameters, string url)
        {
            ObjectListModel result = new ObjectListModel();
            result.Url = url;

            if (!string.IsNullOrEmpty(parameters.Search))
            {
                url += url.Contains("?") ? "&" : "?";

                query = query.Where(a => EF.Property<string>(a.Following, parameters.Filter).Contains(parameters.Search)) as IOrderedQueryable<Follow>;

                url += "Filter=" + parameters.Filter + "&Search=" + parameters.Search;
            }

            result = DataListOperations.ListOperation(result, query, parameters, url);

            return result;
        }

        #endregion
    }
}
