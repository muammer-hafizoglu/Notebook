using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notebook.Business.Managers.Abstract;
using Notebook.Entities.Entities;
using Notebook.Web.Filters;
using Notebook.Web.Models;
using Notebook.Web.Tools.FileManager;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Notebook.Web.Controllers
{
    [TypeFilter(typeof(AccountFilterAttribute))]
    [TypeFilter(typeof(ExceptionFilterAttribute))]
    public class DocumentController : Controller
    {
        private IUserManager _userManager;
        private IFileManager _fileManager;
        private ISettingsManager _settingsManager;
        public DocumentController(IUserManager userManager, IFileManager fileManager, ISettingsManager settingsManager)
        {
            _userManager = userManager;
            _fileManager = fileManager;
            _settingsManager = settingsManager;
        }

        [Route("~/documents")]
        public IActionResult List(string list = "")
        {
            var user = HttpContext.Session.GetSession<User>("User");

            var files = _fileManager.GetFiles(_fileManager.GetWebRootPath() + $"/notebook/users/{user.Username}");

            return View(files);
        }

        [HttpPost]
        [Route("~/add-document")]
        public IActionResult Add(IFormFile file)
        {
            var settings = _settingsManager.Table().FirstOrDefault();

            if (file != null)
            {
                string fileExtension = file.FileName.Substring(file.FileName.LastIndexOf("."));

                if (settings.AcceptedFileTypes.Contains(fileExtension))
                {
                    var user = HttpContext.Session.GetSession<User>("User");

                    _fileManager.Add(new FileModel
                    {
                        FormFile = file,
                        IsWebRoot = true,
                        Name = "",
                        Path = $"/notebook/users/{user.Username}"
                    });
                }
            }

            return Redirect("/documents");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("~/delete-document")]
        public IActionResult Delete(string Url)
        {
            _fileManager.Delete(Url);

            return Redirect("/documents");
        }

        #region Ckeditor

        [Route("~/add-file")]
        public async Task<ActionResult> AddFile()
        {
            var settings = _settingsManager.Table().FirstOrDefault();

            IFormFile file = Request.Form.Files["upload"];

            if (file != null)
            {
                string result = "Failed to load file";

                string fileExtension = file.FileName.Substring(file.FileName.LastIndexOf("."));

                if (settings.AcceptedFileTypes.Contains(fileExtension))
                {
                    var user = HttpContext.Session.GetSession<User>("User");

                    result = _fileManager.Add(new FileModel
                    {
                        FormFile = file,
                        IsWebRoot = true,
                        Name = "",
                        Path = $"/notebook/users/{user.Username}"
                    });
                }

                string CKEditorFuncNum = Request.Query["CKEditorFuncNum"];
                await Response.WriteAsync("<script> window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ",\"" + result + "\");</script>");
                Response.StatusCode = StatusCodes.Status200OK;
            }

            return View();
        }

        [Route("~/get-file")]
        public ActionResult Files()
        {
            var user = HttpContext.Session.GetSession<User>("User");

            var files = _fileManager.GetFiles(_fileManager.GetWebRootPath() + $"/notebook/users/{user.Username}");

            return View(files);
        }

        #endregion
    }
}
