using Microsoft.AspNetCore.Http;
using Notebook.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notebook.Web.Tools.FileManager
{
    public interface IFileManager
    {
        string Add(FileModel fileModel);
        void Delete(string Path);
        List<FileModel> GetFiles(string Path);
        string GetWebRootPath();
        string GetContentRootPath();
    }
}
