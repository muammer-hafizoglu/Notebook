using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
    public class FolderController : Controller
    {
        public string _lockIcon = "<i class='fa fa-lock'></i> ";
        public string _noteIcon = "<i class='fa fa-file-text-o'></i> ";
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
            var sqlQuery = _groupFolderManager.Table()
                .Include(a => a.Group)
                .Include(a => a.Folder)
                .OrderByDescending(a => a.CreateDate)
                .Where(a => a.GroupID == groupId) as IQueryable<GroupFolder>;

            var result = new DatatableResult() { draw = parameters.draw, recordsTotal = sqlQuery.Count() };

            #region Searching
            string search = Request.Form["search[value]"].ToString();
            if (!string.IsNullOrEmpty(search))
            {
                sqlQuery = sqlQuery.Where(a => a.Folder.Name.Contains(search)) as IQueryable<GroupFolder>;
            }

            result.recordsFiltered = sqlQuery.Count();
            #endregion

            result.data = sqlQuery.Skip(parameters.start).Take(parameters.length).Select(_uf =>
                      new FolderModel
                      {
                          name = string.Format("<a href='/{0}/folder/{1}'>{2}  {3}</a>", _uf.Folder.ID, _uf.Folder.Name.ClearHtmlTagAndCharacter(), _uf.Folder.Name, (_uf.Folder.Visible == Visible.Private ? _lockIcon : "")),
                          info = string.Format("{0}: {1}", _noteIcon, _uf.Folder.Notes.Count())
                      }).ToList();

            return Json(result);
        }

        #endregion

        #region CRUD

        [Route("~/{id?}/folder/{title}")]
        public IActionResult Detail(string id = "")
        {
            FolderDetailModel detail = null;

            var _folder = _folderManager.getMany(a => a.ID == id).Include(a => a.Owner).FirstOrDefault();
            if (_folder != null)
            {
                detail = new FolderDetailModel();
                detail.ID = _folder.ID;
                detail.Name = _folder.Name;
                detail.Explanation = _folder.Explanation;
                detail.CreateDate = _folder.CreateDate;
                detail.Visible = _folder.Visible;
                detail.OwnerID = _folder.OwnerID;
                detail.OwnerName = _folder.Owner.Name;
                detail.NoteCount = _folderNoteManager.getMany(a => a.FolderID == _folder.ID).Count();

                var _group = _groupManager.getOne(a => a.ID == TempData["GroupID"].ToString());
                if (_group != null)
                {
                    detail.GroupID = _group.ID;
                    detail.GroupName = _group.Name;
                }
            }

            return View(detail);
        }

        [HttpGet]
        [Route("~/add-folder")]
        [Route("~/{id}/edit-folder")]
        public IActionResult Form(string id = "", string groupId = "")
        {
            var _folder = _folderManager.getOne(a => a.ID == id && a.OwnerID == HttpContext.Session.GetSession<User>("User").ID);

            if (!string.IsNullOrEmpty(groupId)) TempData["GroupID"] = groupId;

            return PartialView(_folder);
        }

        [HttpPost]
        [Route("~/addFolder")]
        public IActionResult Add(Folder _folder)
        {
            var _user = HttpContext.Session.GetSession<User>("User");
            _folder.Owner = _user;

            _folderManager.Add(_folder);
            _folderManager.Save();

            // User - Folder
            _userFolderManager.Add(new UserFolder { Folder = _folder, User = _user, CreateDate = DateTime.Now });
            _userFolderManager.Save();

            // Group-Folder
            if (TempData["GroupID"] != null)
            {
                var _group = _groupManager.getOne(a => a.ID == TempData["GroupID"].ToString());
                if (_group != null)
                {
                    _groupFolderManager.Add(new GroupFolder{ Group = _group, Folder = _folder, CreateDate = DateTime.Now });
                    _groupFolderManager.Save();
                }
                TempData["GroupID"] = null;
            }

            TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "success", message = _localizer["Transaction successful"] });

            return Redirect(TempData["BeforeUrl"].ToString());
        }

        [HttpPost]
        [Route("~/editFolder")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Folder _folder)
        {
            if (!string.IsNullOrEmpty(_folder.ID))
            {
                _folderManager.Update(_folder);
                _folderManager.Save();

                TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "success", message = _localizer["Transaction successful"] });
            }
            else
            {
                TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "error", message = _localizer["Folder not found"] });
            }

            return Redirect(TempData["BeforeUrl"].ToString());
        }

        [HttpGet]
        [Route("~/{id?}/delete-folder")]
        public JsonResult Delete(string id = "")
        {
            var _folder = _folderManager.getOne(a => a.ID == id && a.OwnerID == HttpContext.Session.GetSession<User>("User").ID);
            if (_folder != null)
            {
                _folderManager.Delete(_folder);
                _folderManager.Save();

                TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "success", message = _localizer["Transaction successful"] });
            }
            else
            {
                TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "error", message = _localizer["Folder not found"] });

            }

            return Json("");
        }

        #endregion
    }
}
