using Notebook.Business.Models;
using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;

namespace Notebook.Business.Managers.Abstract
{
    public interface IFolderManager : IManager<Folder>
    {
        void Add(Folder folder, string UserID);
        void Update(Folder folder, string UserID);
        void Delete(string FolderID, string UserID);

        FolderInfoModel GetFolderInfo(string FolderID, string UserID = "");
    }
}
