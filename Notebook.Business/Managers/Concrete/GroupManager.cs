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
        public GroupManager(IGroupDal _servisDal) : base(_servisDal)
        {
            servisDal = _servisDal;
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
            var _group = servisDal.getOne(a => a.ID == model.ID);
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

        public Member MembershipControl(string GroupID, string UserID)
        {
            var _memberType = Member.Visitor;

            var _group = servisDal.getMany(a => a.ID == GroupID).Include(a => a.Users).FirstOrDefault();
            //if (_group != null)
            //{
            //    var _member = _group.Users.Where(a => a.UserID == UserID).FirstOrDefault();
            //    if (_member != null)
            //        _memberType = _member.Status;
            //}

            return _memberType;
        }

        public GroupInfoModel GetGroupInfo(string GroupID, string UserID = "")
        {
            GroupInfoModel group = null;

            var _group = servisDal.getMany(a => a.ID == GroupID)
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
                group.UserCount = _group.Users.Count;
                group.Status = _group.Users.Any(a => a.UserID == UserID) ? _group.Users.FirstOrDefault(a => a.UserID == UserID).Status : Status.Visitor;

                var _owner = _group.Users.FirstOrDefault(a => a.Status == Status.Owner);
                group.UserID = _owner.User.Username;
                group.UserName = _owner.User.Name;
            }

            return group;
        }
    }
}
