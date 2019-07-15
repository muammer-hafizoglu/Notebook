using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Notebook.Web.Models;

namespace Notebook.Web.Tools.FileManager
{
    public class FileManager : IFileManager
    {
        private readonly IHostingEnvironment _environment;
        public FileManager(IHostingEnvironment environment)
        {
            _environment = environment;
        }
        public string Add(FileModel fileModel)
        {
            string path = "";

            if (fileModel.FormFile != null)
            {
                var _file = fileModel.FormFile;

                string fileExtension = _file.FileName.Substring(_file.FileName.LastIndexOf("."));

                string fileName = (!string.IsNullOrEmpty(fileModel.Name) ? fileModel.Name : (_file.FileName.Substring(0, _file.FileName.LastIndexOf(".")) + "_" + DateTime.Now)) + fileExtension;

                try
                {
                    string root = fileModel.IsWebRoot ? GetWebRootPath() + fileModel.Path + "/" : GetContentRootPath() + fileModel.Path + "/";

                    if (!Directory.Exists(root))
                    {
                        Directory.CreateDirectory(root);
                    }

                    root = root + fileName;

                    using (var stream = File.Open(root, FileMode.Create))
                    {
                        _file.CopyTo(stream);
                        stream.Flush();
                    }

                    path = fileModel.Path + "/" + fileName;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return path;
        }

        public void Delete(string Path)
        {
            if (!string.IsNullOrEmpty(Path))
            {
                File.Delete(Path);
            }
        }

        public string GetContentRootPath()
        {
            return _environment.ContentRootPath;
        }

        public string GetWebRootPath()
        {
            return _environment.WebRootPath;
        }
    }
}
