using Notebook.Business.Managers.Abstract;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.Entities.Entities;
using Notebook.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notebook.Business.Managers.Concrete
{
    public class UserGroupManager : Manager<UserGroup>, IUserGroupManager
    {
        private IUserGroupDal servisDal;
        private IGroupManager groupManager;
        private IUserManager userManager;
        public UserGroupManager(IUserGroupDal _servisDal, IGroupManager _groupManager, IUserManager _userManager) : base(_servisDal)
        {
            servisDal = _servisDal;
            groupManager = _groupManager;
            userManager = _userManager;
        }


        public void Join(string GroupID, string UserID)
        {
            var _group = groupManager.getOne(a => a.ID == GroupID);

            if (_group != null && _group.Visible == Visible.Public)
            {
                if (base.getOne(a => a.UserID == UserID && a.GroupID == GroupID) == null)
                {
                    var _user = userManager.getOne(a => a.ID == UserID);
                    if (_user != null)
                    {
                        base.Add(new UserGroup
                        {
                            User = _user,
                            UserID = UserID,
                            Group = _group,
                            GroupID = _group.ID,
                            Status = _group.IsRequiredApproval ? Status.Wait : Status.Member
                        });
                    }
                    else
                    {
                        throw new Exception("User not found");
                    }
                }
            }
            else
            {
                throw new Exception("This group is private");
            }
        }

        public void Exit(string GroupID, string UserID)
        {
            var member = base.getOne(a => a.UserID == UserID && a.GroupID == GroupID);

            if (member != null)
            {
                base.Delete(member);
            }
            else
            {
                throw new Exception("Membership not found");
            }
        }

        public void Delete(string ID, string UserID)
        {
            var member = base.getOne(a => a.ID == ID);

            if (member != null)
            {
                var control = base.getOne(a => a.GroupID == member.GroupID && a.UserID == UserID && a.Status == Status.Owner);

                if (control != null)
                {
                    base.Delete(member);
                }
                else
                {
                    throw new Exception("You do not have sufficient authority to do this");
                }
            }
            else
            {
                throw new Exception("Membership not found");
            }
        }
    }
}
