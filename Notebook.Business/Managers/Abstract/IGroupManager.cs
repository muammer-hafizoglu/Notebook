using Notebook.Business.Models;
using Notebook.Entities.Entities;
using Notebook.Entities.Enums;
using System;
using System.Collections.Generic;

namespace Notebook.Business.Managers.Abstract
{
    public interface IGroupManager : IManager<Group>
    {
        Member MembershipControl(string GroupID, string UserID);

        GroupInfoModel GetGroupInfo(string GroupID, string UserID = "");
        //void Add(Group group, string UserID);
        //void Update(Group group, string UserID);
        //void Delete(string GroupID, string UserID);
    }
}
