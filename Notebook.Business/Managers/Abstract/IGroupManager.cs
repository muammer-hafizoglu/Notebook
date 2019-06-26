using Notebook.Entities.Entities;
using Notebook.Entities.Enums;
using System;
using System.Collections.Generic;

namespace Notebook.Business.Managers.Abstract
{
    public interface IGroupManager : IManager<Group>
    {
        Member MembershipControl(string GroupID, string UserID);
    }
}
