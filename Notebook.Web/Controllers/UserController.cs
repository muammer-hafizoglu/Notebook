using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notebook.Business.Managers.Abstract;
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
        private IUserFolderManager _userFolderManager;
        private IUserNoteManager _userNoteManager;
        public UserController(IUserManager userManager, IUserGroupManager userGroupManager, IUserFolderManager userFolderManager, IUserNoteManager userNoteManager)
        {
            _userManager = userManager;
            _userGroupManager = userGroupManager;
            _userFolderManager = userFolderManager;
            _userNoteManager = userNoteManager;
        }

        [Route("~/profile/{id}/{name}/{list?}")]
        public IActionResult Profile(string id = "",string list = "")
        {
            User _user = null;
            UserProfileModel userModel = null;

            if (!string.IsNullOrEmpty(id))
                _user = _userManager.getOne(a => a.ID == id);
            else
                _user = HttpContext.Session.GetSession<User>("User");

            if (_user != null)
            {
                userModel = new UserProfileModel();

                userModel.ID = _user.ID;
                userModel.Name = _user.Name;
                userModel.Info = _user.Info;
                userModel.CreateDate = _user.CreateDate;
                userModel.Avatar = _user.Avatar;
                userModel.GroupCount = _userGroupManager.getMany(a => a.UserID == _user.ID).Count();
                userModel.FolderCount = _userFolderManager.getMany(a => a.UserID == _user.ID).Count();
                userModel.NoteCount = _userNoteManager.getMany(a => a.UserID == _user.ID).Count();
                userModel.List = list;
            }

            return View(userModel);
        }
    }
}
