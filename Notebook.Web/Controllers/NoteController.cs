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
    public class NoteController : Controller
    {
        public string _lockIcon = "<i class='fa fa-lock'></i> ";
        public string _noteIcon = "<i class='fa fa-newspaper-o'></i> ";

        private IStringLocalizer<NoteController> _localizer;
        private INoteManager _noteManager;
        private IUserNoteManager _userNoteManager;
        private IGroupNoteManager _groupNoteManager;
        private IFolderNoteManager _folderNoteManager;
        private IGroupManager _groupManager;
        private IFolderManager _folderManager;
        public NoteController(IStringLocalizer<NoteController> localizer, INoteManager noteManager, IGroupNoteManager groupNoteManager, 
            IFolderNoteManager folderNoteManager, IGroupManager groupManager, IFolderManager folderManager, IUserNoteManager userNoteManager)
        {
            _localizer = localizer;
            _noteManager = noteManager;
            _userNoteManager = userNoteManager;
            _groupNoteManager = groupNoteManager;
            _folderNoteManager = folderNoteManager;
            _groupManager = groupManager;
            _folderManager = folderManager;
        }

        #region List

        [HttpPost]
        [Route("~/user-notes")]
        public JsonResult UserNotes(DatatableParameters parameters, string userId = "")
        {
            var _activeUser = HttpContext.Session.GetSession<User>("User");

            var sqlQuery = _userNoteManager.getUserNotes(userId,(_activeUser != null && _activeUser.ID == userId) ? true:false);

            var result = new DatatableResult() { draw = parameters.draw, recordsTotal = sqlQuery.Count() };

            //Searching
            string searchText = Request.Form["search[value]"].ToString();
            sqlQuery = sqlQuery.Where(a => a.Note.Title.Contains(searchText)) as IQueryable<UserNote>;
            result.recordsFiltered = sqlQuery.Count();

            result.data = sqlQuery.Skip(parameters.start).Take(parameters.length).Select(_un =>
                     new NoteModel
                     {
                         name = string.Format("<a href='/{0}/note/{1}'>{4} {2} {3}</a>",
                                    _un.Note.ID, _un.Note.Title.ClearHtmlTagAndCharacter(), _un.Note.Title, (_un.Note.Visible == Visible.Private ? _lockIcon : ""), _noteIcon)
                     }).ToList();

            return Json(result);
        }

        [HttpPost]
        [Route("~/group-notes")]
        public JsonResult GroupNotes(DatatableParameters parameters, string groupId = "")
        {
            var sqlQuery = _groupNoteManager.Table()
                .Include(a => a.Group)
                .Include(a => a.Note)
                .OrderByDescending(a => a.Note.CreateDate)
                .Where(a => a.Group.ID == groupId)
                as IQueryable<GroupNote>;

            var result = new DatatableResult() { draw = parameters.draw, recordsTotal = sqlQuery.Count() };

            //Searching
            string searchText = Request.Form["search[value]"].ToString();
            sqlQuery = sqlQuery.Where(a => a.Note.Title.Contains(searchText)) as IQueryable<GroupNote>;
            result.recordsFiltered = sqlQuery.Count();

            result.data = sqlQuery.Skip(parameters.start).Take(parameters.length).Select(_gn =>
                      new NoteModel
                      {
                          name = string.Format("<a href='/{0}/note/{1}'>{4} {2} {3}</a>",
                                    _gn.Note.ID, _gn.Note.Title.ClearHtmlTagAndCharacter(), _gn.Note.Title, (_gn.Note.Visible == Visible.Private ? _lockIcon : ""), _noteIcon)
                      }).ToList();

            return Json(result);
        }

        [HttpPost]
        [Route("~/folder-notes")]
        public JsonResult FolderNotes(DatatableParameters parameters, string folderId = "")
        {
            var sqlQuery = _folderNoteManager.Table()
                .Include(a => a.Folder)
                .Include(a => a.Note)
                .OrderByDescending(a => a.Note.CreateDate)
                .Where(a => a.Folder.ID == folderId)
                as IQueryable<FolderNote>;

            var result = new DatatableResult() { draw = parameters.draw, recordsTotal = sqlQuery.Count() };

            //Searching
            string searchText = Request.Form["search[value]"].ToString();
            sqlQuery = sqlQuery.Where(a => a.Note.Title.Contains(searchText)) as IQueryable<FolderNote>;
            result.recordsFiltered = sqlQuery.Count();

            result.data = sqlQuery.Skip(parameters.start).Take(parameters.length).Select(_fn =>
                      new NoteModel
                      {
                          name = string.Format("<a href='/{0}/note/{1}'>{4} {2} {3}</a>",
                                    _fn.Note.ID, _fn.Note.Title.ClearHtmlTagAndCharacter(), _fn.Note.Title, (_fn.Note.Visible == Visible.Private ? _lockIcon : ""), _noteIcon)
                      }).ToList();

            return Json(result);
        }

        #endregion

        #region CRUD

        [HttpGet]
        [Route("~/{id?}/note/{title?}")]
        [Route("~/{id?}/note/{list?}")]
        public IActionResult Detail(string id = "", string list = "")
        {
            NoteDetailModel detail = null;

            var _note = _noteManager.getMany(a => a.ID == id).Include(a => a.Owner).FirstOrDefault();
            if (_note != null)
            {
                _note.ReadCount += 1;

                detail = new NoteDetailModel();
                detail.ID = _note.ID;
                detail.Title = _note.Title;
                detail.Explanation = _note.Explanation;
                detail.Content = _note.Content;
                detail.CreateDate = _note.CreateDate;
                detail.UpdateDate = _note.UpdateDate;
                detail.Visible = _note.Visible;
                detail.OwnerID = _note.OwnerID;
                detail.OwnerName = _note.Owner.Name;
                detail.ReadCount = _note.ReadCount;
                detail.UserCount = _userNoteManager.getMany(a => a.NoteID == _note.ID).Count();
                detail.List = list;

                _noteManager.Update(_note);
                _noteManager.Save();
            }

            return View(detail);
        }

        [HttpGet]
        [Route("~/add-note")]
        [Route("~/{id?}/edit-note")]
        public IActionResult Form(string id = "",string groupId = "", string folderId = "")
        {
            var _sessionUser = HttpContext.Session.GetSession<User>("User");
            var _note = _noteManager.getOne(a => a.ID == id && a.OwnerID == _sessionUser.ID);

            if (!string.IsNullOrEmpty(groupId)) TempData["GroupID"] = groupId;
            if (!string.IsNullOrEmpty(folderId)) TempData["FolderID"] = folderId;

            return PartialView(_note ?? new Note { Visible = Visible.Public });
        }

        [HttpPost]
        [Route("~/addNote")]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Note _note)
        {
            var _user = HttpContext.Session.GetSession<User>("User");
            _note.Owner = _user;

            _noteManager.Add(_note);
            _noteManager.Save();

            // User - Note
            _userNoteManager.Add(new UserNote { User = _user, Note = _note, CreateDate = DateTime.Now, Member = Member.Owner });
            _userNoteManager.Save();

            // Group-Note
            if (TempData["GroupID"] != null)
            {
                var _group = _groupManager.getOne(a => a.ID == TempData["GroupID"].ToString());
                if (_group != null)
                {
                    _groupNoteManager.Add(new GroupNote { Group = _group, Note = _note, CreateDate = DateTime.Now });
                    _groupNoteManager.Save();
                }
            }

            // Folder-Note
            if (TempData["FolderID"] != null)
            {
                var _folder = _folderManager.getOne(a => a.ID == TempData["FolderID"].ToString());
                if (_folder != null)
                {
                    _folderNoteManager.Add(new FolderNote { Folder = _folder, Note = _note, CreateDate = DateTime.Now });
                    _folderNoteManager.Save();
                }
            }

            TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "success", message = _localizer["Transaction successful"] });

            return Redirect(TempData["BeforeUrl"].ToString());
        }

        [HttpPost]
        [Route("~/editNote")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Note note)
        {
            _noteManager.Update(note);
            _noteManager.Save();

            TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "success", message = _localizer["Transaction successful"] });

            return Redirect(string.Format("/{0}/note/{1}",note.ID,note.Title.ClearHtmlTagAndCharacter()));
        }

        [HttpPost]
        [Route("~/{id?}/delete-note")]
        public JsonResult Delete(string id = "")
        {
            var _note = _noteManager.getOne(a => a.ID == id && a.OwnerID == HttpContext.Session.GetSession<User>("User").ID);
            if (_note != null)
            {
                _noteManager.Delete(_note);
                _noteManager.Save();

                TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "success", message = _localizer["Transaction successful"] });
            }
            else
            {
                TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "error", message = _localizer["Note not found"] });

            }

            return Json("");
        }

        #endregion

    }
}
