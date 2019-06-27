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
        public string _lockIcon = "<i class='fa fa-lock'></i> ";
        public string _noteIcon = "<i class='fa fa-newspaper-o'></i> ";
        public string _userIcon = "<i class='fa fa-user'></i> ";

        private IStringLocalizer<FolderController> _localizer;
        private IFolderManager _folderManager;
        private IGroupManager _groupManager;
        private IUserFolderManager _userFolderManager;
        private IGroupFolderManager _groupFolderManager;
        private IFolderNoteManager _folderNoteManager;
        public FolderController(IStringLocalizer<FolderController> localizer,IFolderManager folderManager, IUserFolderManager userFolderManager, 
            IGroupFolderManager groupFolderManager, IGroupManager groupManager, IFolderNoteManager folderNoteManager)
        {
            _localizer = localizer;
            _folderManager = folderManager;
            _groupManager = groupManager;
            _userFolderManager = userFolderManager;
            _groupFolderManager = groupFolderManager;
            _folderNoteManager = folderNoteManager;
        }

        #region List

        [HttpPost]
        [Route("~/group-folders")]
        public JsonResult GroupFolders(DatatableParameters parameters, string groupId = "")
        {
            var sqlQuery = _folderManager.Table()
                .Include(a => a.Group)
                .Include(a => a.Notes)
                .OrderByDescending(a => a.CreateDate)
                .Where(a => a.Group.ID == groupId) as IQueryable<Folder>;

            var result = new DatatableResult() { draw = parameters.draw, recordsTotal = sqlQuery.Count() };

            #region Searching
            string search = Request.Form["search[value]"].ToString();
            if (!string.IsNullOrEmpty(search))
            {
                sqlQuery = sqlQuery.Where(a => a.Name.Contains(search)) as IQueryable<Folder>;
            }

            result.recordsFiltered = sqlQuery.Count();
            #endregion

            result.data = sqlQuery.Skip(parameters.start).Take(parameters.length).Select(a =>
                      new FolderModel
                      {
                          name = string.Format("<a href='/folder/{0}/{1}'>{2}  {3}</a>", a.ID, a.Name.ClearHtmlTagAndCharacter(), a.Name, (a.Visible == Visible.Private ? _lockIcon : "")),
                          info = string.Format("{0}: {1}", _noteIcon, a.Notes.Count())
                      }).ToList();

            return Json(result);
        }

        #endregion

        #region CRUD

        [Route("~/folder/{ID}/{title}")]
        public IActionResult Detail(string ID = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            FolderDetailModel detail = null;
            
            var _folder = _folderManager.getMany(a => a.ID == ID).Include(a => a.Group).ThenInclude(b => b.Users).FirstOrDefault();
            if (_folder != null)
            {
                detail = new FolderDetailModel();
                detail.ID = _folder.ID;
                detail.Name = _folder.Name;
                detail.Explanation = _folder.Explanation;
                detail.CreateDate = _folder.CreateDate;
                detail.Visible = _folder.Visible;
                detail.Group = _folder.Group;
                detail.NoteCount = _folderNoteManager.getMany(a => a.FolderID == _folder.ID).Count();

                var _member = _folder.Group.Users.Where(a => a.UserID == _user?.ID).FirstOrDefault();
                detail.MemberType = (_member != null) ? _member.MemberType : Member.Visitor;
            }

            return View(detail);
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
