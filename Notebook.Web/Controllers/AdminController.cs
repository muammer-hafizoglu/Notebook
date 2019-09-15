using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Notebook.Business.Managers.Abstract;
using Notebook.Entities.Entities;
using Notebook.Web.Filters;
using Notebook.Web.Models;
using Notebook.Web.Models.Datatable;
using Notebook.Web.Tools.FileManager;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Notebook.Web.Controllers
{
    [TypeFilter(typeof(AccountFilterAttribute), Arguments = new[] { "VIEW_ADMINPANEL" })]
    public class AdminController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IFileManager _fileManager;
        private readonly IUserManager _userManager;
        private readonly ISettingsManager _settingsManager;
        public AdminController(IConfiguration configuration,IUserManager userManager, ISettingsManager settingsManager, IFileManager fileManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _settingsManager = settingsManager;
            _fileManager = fileManager;
        }

        #region Notebook Settings

        [Route("~/notebook-settings")]
        [TypeFilter(typeof(AccountFilterAttribute), Arguments = new[] { "EDIT_SETTINGS" })]
        public IActionResult Settings()
        {
            var settings = _settingsManager.Table().FirstOrDefault();

            return View(settings ?? new Settings());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("~/update-settings")]
        public IActionResult Settings(Settings settings, IFormFile Logo, IFormFile Icon)
        {
            if (Logo != null)
            {
                _fileManager.Delete(_fileManager.GetWebRootPath() + settings.Logo);

                settings.Logo = _fileManager.Add(new FileModel
                {
                    FormFile = Logo,
                    IsWebRoot = true,
                    Name = "logo",
                    Path = "/notebook/images"
                });
            }

            if (Icon != null)
            {
                _fileManager.Delete(_fileManager.GetWebRootPath() + settings.Icon);

                settings.Icon = _fileManager.Add(new FileModel
                {
                    FormFile = Icon,
                    IsWebRoot = true,
                    Name = "favicon",
                    Path = ""
                });
            }

            _settingsManager.Update(settings);

            return Redirect("/notebook-settings");
        }

        #endregion

        #region Userlist

        [TypeFilter(typeof(AccountFilterAttribute), Arguments = new[] { "VIEW_USERS" })]
        [Route("~/notebook-membership")]
        public IActionResult Users(Parameters parameters)
        {
            var list = DataListOperations.List(_userManager.Table().OrderByDescending(a => a.CreateDate), parameters, "/notebook-membership");

            return View(list);
        }

        #endregion

        #region Datatable User List

        //[TypeFilter(typeof(AccountFilterAttribute), Arguments = new[] { "VIEW_USERS" })]
        //[HttpPost]
        //[Route("~/notebook-userlist")]
        //public JsonResult UserList(DatatableParameters parameters)
        //{
        //    var result = new DatatableResult() { draw = parameters.draw, recordsTotal = _userManager.Table().Count() };

        //    var sqlQuery = _userManager.Table();

        //    sqlQuery = Searching(sqlQuery);
        //    sqlQuery = Ordering(sqlQuery);

        //    result.recordsFiltered = sqlQuery.Count();

        //    result.data = sqlQuery.Skip(parameters.start).Take(parameters.length).Select(_user =>
        //              new UserModel
        //              {
        //                  Name = _user.Name,
        //                  Username = _user.Username,
        //                  Email = _user.Email,
        //                  State = _user.Approve ? "<label class='label label-success'>Active</label>" : "<label class='label label-warning'>Passive</label>",
        //                  Operations = "<button class='btn btn-xxs btn-primary' nb-type='modal' nb-method='get' nb-cont='Admin' nb-act='EditUserPartial' nb-id='" + _user.ID + "'><i class='fa fa-edit'></i></button>" +
        //                  "<button class='btn btn-xxs btn-danger' nb-type='modal' nb-method='get' nb-cont='Admin' nb-act='RemoveUser' nb-id='" + _user.ID + "'><i class='fa fa-remove'></i></button>"
        //              }).ToList();

        //    return Json(result);
        //}

        //private IQueryable<User> Searching(IQueryable<User> sqlQuery)
        //{
        //    var _sqlQuery = sqlQuery;

        //    string search = Request.Form["search[value]"].ToString();
        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        _sqlQuery = _sqlQuery.Where(a => a.Name.Contains(search) || a.Username.Contains(search)) as IQueryable<User>;
        //    }

        //    return _sqlQuery;
        //}

        //private IQueryable<User> Ordering(IQueryable<User> sqlQuery)
        //{
        //    var _sqlQuery = sqlQuery;

        //    string orderColumn = Request.Form["order[0][column]"].ToString();
        //    if (!string.IsNullOrEmpty(orderColumn))
        //    {
        //        string orderType = Request.Form["order[0][dir]"].ToString();
        //        switch (orderColumn)
        //        {
        //            case "0":
        //                {
        //                    _sqlQuery = (orderType == "asc") ? _sqlQuery.OrderBy(a => a.Name) as IQueryable<User> : _sqlQuery.OrderByDescending(a => a.Name) as IQueryable<User>;
        //                    break;
        //                }
        //            case "1":
        //                {
        //                    _sqlQuery = (orderType == "asc") ? _sqlQuery.OrderBy(a => a.Username) as IQueryable<User> : _sqlQuery.OrderByDescending(a => a.Username) as IQueryable<User>;
        //                    break;
        //                }
        //            case "2":
        //                {
        //                    _sqlQuery = (orderType == "asc") ? _sqlQuery.OrderBy(a => a.Email) as IQueryable<User> : _sqlQuery.OrderByDescending(a => a.Email) as IQueryable<User>;
        //                    break;
        //                }
        //            case "3":
        //                {
        //                    _sqlQuery = (orderType == "asc") ? _sqlQuery.OrderBy(a => a.Approve) as IQueryable<User> : _sqlQuery.OrderByDescending(a => a.Approve) as IQueryable<User>;
        //                    break;
        //                }
        //        }

        //    }

        //    return _sqlQuery;
        //}

        #endregion

        #region Edit User

        [TypeFilter(typeof(AccountFilterAttribute), Arguments = new[] { "EDIT_USERS" })]
        [HttpGet]
        [Route("~/edit-user/{ID}")]
        public IActionResult UserForm(string ID = "")
        {
            var _user = _userManager.getOne(a => a.ID == ID);

            return PartialView(_user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("~/editUser")]
        public IActionResult EditUser(User model, string NewPassword)
        {
            if (!string.IsNullOrEmpty(NewPassword))
            {
                model.Password = NewPassword.SHA256Encrypt();
            }

            _userManager.Update(model);

            return Redirect("/notebook-membership");
        }
        #endregion

    }
}
