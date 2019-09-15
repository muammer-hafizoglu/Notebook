using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;

namespace Notebook.Business.Managers.Abstract
{
    public interface IUserGroupManager : IManager<UserGroup>
    {
        void Join(string GroupID, string UserID);
        void Exit(string GroupID, string UserID);
        void Delete(string ID, string UserID);
    }
}
