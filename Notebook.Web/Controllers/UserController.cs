using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notebook.Business.Managers.Abstract;
using Notebook.Business.Models;
using Notebook.Entities.Entities;
using Notebook.Entities.Enums;
using Notebook.Web.Filters;
using Notebook.Web.Models;
using Notebook.Web.Models.Datatable;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Notebook.Web.Controllers
{
    [TypeFilter(typeof(ExceptionFilterAttribute))]
    public class UserController : Controller
    {
        private IUserManager _userManager;
        private IUserGroupManager _userGroupManager;
        private IUserNoteManager _userNoteManager;
        public UserController(IUserManager userManager, IUserGroupManager userGroupManager, IUserNoteManager userNoteManager)
        {
            _userManager = userManager;
            _userGroupManager = userGroupManager;
            _userNoteManager = userNoteManager;
        }

        private UserInfoModel GetUser(string ID)
        {
            //UserInfoModel user = null;

            //var _user = _userManager.getOne(a => a.Username == ID);

            //if (_user != null)
            //{
            //    user = new UserInfoModel();
            //    user.ID = _user.ID;
            //    user.Username = _user.Username;
            //    user.Name = _user.Name;
            //    user.Info = _user.Info;
            //    user.CreateDate = _user.CreateDate;
            //    user.Avatar = _user.Avatar;
            //    user.Lock = _user.Lock;
            //    user.GroupCount = _userGroupManager.getMany(a => a.UserID == _user.ID).Count();
            //    user.NoteCount = _userNoteManager.getMany(a => a.UserID == _user.ID).Count();
            //}

            //return user;
            return _userManager.GetUserInfo(ID);
        }

        [Route("~/{ID}")]
        public IActionResult Timeline(string ID = "")
        {
            ProfileModel model = new ProfileModel();

            model.User = GetUser(ID);
            model.Navigation = new NavigationModel { List = "Timeline", ID = ID };
            //model.Data = 

            return View(model);
        }

        [Route("~/{ID}/groups")]
        public IActionResult Groups(Parameters parameters)
        {
            ProfileModel model = new ProfileModel();

            model.User = GetUser(parameters.ID);
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

            return View(model);
        }

        private ObjectListModel GroupList(IQueryable<UserGroup> query, Parameters parameters, string url)
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
                            query = query.Where(a => a.Group.ID == parameters.Search) as IOrderedQueryable<UserGroup>;
                            break;
                        }
                    case "Name":
                        {
                            query = query.Where(a => a.Group.Name.Contains(parameters.Search)) as IOrderedQueryable<UserGroup>;
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

        [Route("~/{ID}/notes")]
        public IActionResult Notes(Parameters parameters)
        {
            ProfileModel model = new ProfileModel();

            model.User = GetUser(parameters.ID);
            model.Navigation = new NavigationModel { List = "Notes", ID = parameters.ID };
            model.Data = NoteList(
                _userNoteManager.Table()
                    .Where(a => a.UserID == model.User.ID)
                    .Include(a => a.User)
                    .Include(a => a.Note)
                    .OrderByDescending(a => a.CreateDate),
                parameters,
                $"/{parameters.ID}/notes");

            return View(model);
        }

        private ObjectListModel NoteList(IQueryable<UserNote> query, Parameters parameters, string url)
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
                            query = query.Where(a => a.Note.ID == parameters.Search) as IOrderedQueryable<UserNote>;
                            break;
                        }
                    case "Title":
                        {
                            query = query.Where(a => a.Note.Title.Contains(parameters.Search)) as IOrderedQueryable<UserNote>;
                            break;
                        }
                    case "Content":
                        {
                            query = query.Where(a => a.Note.Content.Contains(parameters.Search)) as IOrderedQueryable<UserNote>;
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
    }
}
