using Notebook.Business.Models;
using Notebook.Entities.Entities;
using Notebook.Entities.Enums;
using System;
using System.Collections.Generic;

namespace Notebook.Business.Managers.Abstract
{
    public interface IGroupManager : IManager<Group>
    {
        void Delete(string GroupID, string UserID);

        GroupInfoModel GetGroupInfo(string GroupID, string UserID = "");
    }
}
