using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
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
    public class NoteController : Controller
    {
        private INoteManager _noteManager;
        private IUserNoteManager _userNoteManager;
        private IGroupManager _groupManager;
        private IFolderManager _folderManager;
        private IEventManager _eventManager;
        private IFileManager _fileManager;
        public NoteController(INoteManager noteManager, IGroupManager groupManager, IFolderManager folderManager, 
            IUserNoteManager userNoteManager, IEventManager eventManager, IFileManager fileManager)
        {
            _noteManager = noteManager;
            _userNoteManager = userNoteManager;
            _groupManager = groupManager;
            _folderManager = folderManager;
            _eventManager = eventManager;
            _fileManager = fileManager;
        }

        #region Note CRUD

        [HttpGet]
        [Route("~/{ID}/note-detail/{title?}")]
        public IActionResult Detail(string ID = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            NoteInfoModel detail = _noteManager.GetNoteInfo(ID, _user?.ID);
            if (detail != null)
            {
                detail.Group = (detail.Group != null) ? _groupManager.GetGroupInfo(detail.Group.ID, _user?.ID) :
                (detail.Folder != null) ? _groupManager.GetGroupInfo(detail.Folder.GroupID, _user?.ID) : null;
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

            ViewBag.FileUpload = _sessionUser.CanUploadFile;

            var _note = _noteManager.getOne(a => a.ID == id && a.UserID == _sessionUser.ID);

            return PartialView(_note ?? new Note { Visible = Visible.Public, UserID = _sessionUser.ID, GroupID = groupId, FolderID = folderId });
        }

        [TypeFilter(typeof(AccountFilterAttribute))]
        [HttpPost]
        [Route("~/addNote")]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Note _note, string View = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            _noteManager.Add(_note);
            _userNoteManager.Add(new UserNote{ Note = _note, User = _user, CreateDate = DateTime.Now, Status = Status.Owner });

            _eventManager.Add(new Event
            {
                User = _user,
                View = View == "on" ? true : false,
                Url = $"{_note.ID}/note-detail",
                ProductID = _note.ID,
                ProductName = _note.Title,
                Type = Product.Note,
                Explation = "New note added"
            });

            TempData["message"] = HelperMethods.ObjectConvertJson(new TempDataModel { type = "success", message = "Transaction successful" });

            return Redirect(TempData["BeforeUrl"].ToString());
        }

        [TypeFilter(typeof(AccountFilterAttribute))]
        [HttpPost]
        [Route("~/editNote")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Note note)
        {
            _noteManager.Update(note);

            TempData["message"] = HelperMethods.ObjectConvertJson(new TempDataModel { type = "success", message = "Transaction successful" });

            return Redirect(string.Format("/{0}/note-detail/{1}",note.ID,note.Title.ClearHtmlTagAndCharacter()));
        }

        [TypeFilter(typeof(AccountFilterAttribute))]
        [HttpGet]
        [Route("~/{ID?}/delete-note")]
        public JsonResult Delete(string ID = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            _noteManager.Delete(ID, _user.ID);

            TempData["message"] = HelperMethods.ObjectConvertJson(new TempDataModel { type = "success", message = "Transaction successful" });

            return Json("");
        }

        #endregion

    }
}
