using Microsoft.EntityFrameworkCore;
using Notebook.Business.Managers.Abstract;
using Notebook.Business.Models;
using Notebook.Business.Tools.Validation.FluentValidation;
using Notebook.Core.Aspects.SimpleProxy.Validation;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.Entities.Entities;
using Notebook.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Notebook.Business.Managers.Concrete
{
    public class GroupManager : Manager<Group>, IGroupManager
    {
        private IGroupDal servisDal;
        private IUserManager userManager;
        public GroupManager(IGroupDal _servisDal, IUserManager _userManager) : base(_servisDal)
        {
            servisDal = _servisDal;
            userManager = _userManager;
        }

        [Validate(typeof(Group), typeof(GroupFluentValidation))]
        public override void Add(Group model)
        {
            model.CreateDate = DateTime.Now;

            base.Add(model);
        }

        [Validate(typeof(Group), typeof(GroupFluentValidation))]
        public override void Update(Group model)
        {
            var _group = base.getOne(a => a.ID == model.ID);
            if (_group != null)
            {
                // TODO: AutoMapper Uygulanacak
                _group.Name = model.Name;
                _group.Explanation = model.Explanation;
                _group.Visible = model.Visible;
                _group.IsRequiredApproval = model.IsRequiredApproval;

                base.Update(_group);
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        public void Delete(string GroupID, string UserID)
        {
            var _group = base.getMany(a => a.ID == GroupID)
                .Include(a => a.Users)
                    .ThenInclude(b => b.User)
                .FirstOrDefault();
            if (_group != null)
            {
                var _member = _group.Users.Where(a => a.UserID == UserID).FirstOrDefault();
                if (_member != null && (_member.Status == Status.Owner || _member.Status == Status.Manager))
                {
                    base.Delete(_group);
                }
                else
                {
                    throw new Exception("Authorization error");
                }
            }
            else
            {
                throw new Exception("Group not found");
            }
        }

        public GroupInfoModel GetGroupInfo(string GroupID, string UserID = "")
        {
            GroupInfoModel group = null;

            var _group = base.getMany(a => a.ID == GroupID)
                .Include(a => a.Users)
                    .ThenInclude(b => b.User)
                .Include(a => a.Folders)
                .Include(a => a.Notes)
                .FirstOrDefault();

            if (_group != null)
            {
                group = new GroupInfoModel();
                group.ID = _group.ID;
                group.Name = _group.Name;
                group.Explanation = _group.Explanation;
                group.CreateDate = _group.CreateDate;
                group.Visible = _group.Visible;
                group.FolderCount = _group.Folders.Count;
                group.NoteCount = _group.Notes.Count;
                group.UserCount = _group.Users.Where(a => a.Status == Status.Member || a.Status == Status.Manager).Count();
                group.WaitingUser = _group.Users.Where(a => a.Status == Status.Wait).Count();
                group.User = _group.Users.FirstOrDefault(a => a.Status == Status.Owner).User;

                var user = _group.Users.FirstOrDefault(a => a.UserID == UserID);
                group.Status = user != null ? user.Status : !string.IsNullOrEmpty(UserID) ? Status.User : Status.Visitor;
            }

            return group;
        }

    }
}
