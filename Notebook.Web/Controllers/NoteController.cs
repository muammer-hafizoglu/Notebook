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
using Notebook.Web.Filters;
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
            var sqlQuery = _noteManager.Table()
                .OrderByDescending(a => a.CreateDate)
                .Where(a => a.OwnerID == userId)
                as IQueryable<Note>;

            DatatableResult result = NoteList(parameters, ref sqlQuery);

            return Json(result);
        }

        [HttpPost]
        [Route("~/group-notes")]
        public JsonResult GroupNotes(DatatableParameters parameters, string groupId = "")
        {
            var sqlQuery = _noteManager.Table()
                .OrderByDescending(a => a.CreateDate)
                .Where(a => a.GroupID == groupId)
                as IQueryable<Note>;

            DatatableResult result = NoteList(parameters, ref sqlQuery);

            return Json(result);
        }

        [HttpPost]
        [Route("~/folder-notes")]
        public JsonResult FolderNotes(DatatableParameters parameters, string folderId = "")
        {
            var sqlQuery = _noteManager.Table()
                .OrderByDescending(a => a.CreateDate)
                .Where(a => a.FolderID == folderId)
                as IQueryable<Note>;

            DatatableResult result = NoteList(parameters, ref sqlQuery);

            return Json(result);
        }

        private DatatableResult NoteList(DatatableParameters parameters, ref IQueryable<Note> sqlQuery)
        {
            var result = new DatatableResult() { draw = parameters.draw, recordsTotal = sqlQuery.Count() };

            //Searching
            string searchText = Request.Form["search[value]"].ToString();

            sqlQuery = sqlQuery.Where(a => a.Title.Contains(searchText)) as IQueryable<Note>;

            result.recordsFiltered = sqlQuery.Count();
            result.data = sqlQuery.Skip(parameters.start).Take(parameters.length).Select(note =>
                        new NoteModel
                        {
                            name = string.Format("<a href='/note/{0}/{1}'>{4} {2} {3}</a>",
                                    note.ID, note.Title.ClearHtmlTagAndCharacter(), note.Title, (note.Visible == Visible.Private ? _lockIcon : ""), _noteIcon)
                        }).ToList();
            return result;
        }

        #endregion

        #region CRUD

        [HttpGet]
        [Route("~/note/{id}/{title?}")]
        public IActionResult Detail(string id = "", string list = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            NoteDetailModel detail = null;

            var _note = _noteManager.getMany(a => a.ID == id).Include(a => a.Owner).FirstOrDefault();
            if (_note != null)
            {
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

                _noteManager.UpdateNoteReadCount(_note);
            }

            return View(detail);
        }

        [TypeFilter(typeof(AccountFilterAttribute))]
        [HttpGet]
        [Route("~/add-note")]
        [Route("~/{id?}/edit-note")]
        public IActionResult Form(string id = "",string groupId = "", string folderId = "")
        {
            var _sessionUser = HttpContext.Session.GetSession<User>("User");

            var _note = _noteManager.getOne(a => a.ID == id && a.OwnerID == _sessionUser.ID);

            return PartialView(_note ?? new Note { Visible = Visible.Public, OwnerID = _sessionUser.ID, GroupID = groupId, FolderID = folderId });
        }

        [TypeFilter(typeof(AccountFilterAttribute))]
        [HttpPost]
        [Route("~/addNote")]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Note _note)
        {
            _noteManager.Add(_note);

            TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "success", message = _localizer["Transaction successful"] });

            return Redirect(TempData["BeforeUrl"].ToString());
        }

        [TypeFilter(typeof(AccountFilterAttribute))]
        [HttpPost]
        [Route("~/editNote")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Note note)
        {
            _noteManager.Update(note);

            TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "success", message = _localizer["Transaction successful"] });

            return Redirect(string.Format("/note/{0}/{1}",note.ID,note.Title.ClearHtmlTagAndCharacter()));
        }

        [TypeFilter(typeof(AccountFilterAttribute))]
        [HttpPost]
        [Route("~/{ID?}/delete-note")]
        public JsonResult Delete(string ID = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            _noteManager.Delete(ID, _user.ID);

            TempData["message"] = HelperMethods.JsonConvertString(new TempDataModel { type = "success", message = _localizer["Transaction successful"] });

            return Json("");
        }

        #endregion

    }
}
