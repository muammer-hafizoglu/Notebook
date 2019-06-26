using Microsoft.EntityFrameworkCore;
using Notebook.Business.Managers.Abstract;
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
    public class FolderManager : Manager<Folder>, IFolderManager
    {
        private IFolderDal servisDal;
        private IGroupDal groupDal;
        public FolderManager(IFolderDal _servisDal, IGroupDal _groupDal) : base(_servisDal)
        {
            servisDal = _servisDal;
            groupDal = _groupDal;
        }

        [Validate(typeof(Folder), typeof(FolderFluentValidation))]
        public void Add(Folder model,string UserID = "")
        {
            var _group = groupDal.getMany(a => a.ID == model.Group.ID).Include(a => a.Users).FirstOrDefault();
            if (_group != null)
            {
                var _member = _group.Users.Where(a => a.UserID == UserID).FirstOrDefault();
                if (_member != null && (_member.MemberType == Member.Owner || _member.MemberType == Member.Manager))
                {
                    model.Group = _group;
                    model.CreateDate = DateTime.Now;

                    base.Add(model);
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

        public void Delete(string FolderID, string UserID)
        {
            var _folder = servisDal.getMany(a => a.ID == FolderID).Include(a => a.Group).ThenInclude(b => b.Users).FirstOrDefault();
            if (_folder != null)
            {
                var _member = _folder.Group.Users.Where(a => a.UserID == UserID).FirstOrDefault();
                if (_member != null && (_member.MemberType == Member.Owner || _member.MemberType == Member.Manager))
                {
                    base.Delete(_folder);
                }
                else
                {
                    throw new Exception("Authorization error");
                }
            }
            else
            {
                throw new Exception("Folder not found");
            }
        }

        [Validate(typeof(Folder), typeof(FolderFluentValidation))]
        public void Update(Folder model, string UserID)
        {
            var _group = groupDal.getMany(a => a.ID == model.Group.ID).Include(a => a.Users).FirstOrDefault();
            if (_group != null)
            {
                var _member = _group.Users.Where(a => a.UserID == UserID).FirstOrDefault();
                if (_member != null && (_member.MemberType == Member.Owner || _member.MemberType == Member.Manager))
                {
                    var _folder = servisDal.getOne(a => a.ID == model.ID);
                    if (_folder != null)
                    {
                        _folder.Name = model.Name;
                        _folder.Explanation = model.Explanation;
                        _folder.Visible = model.Visible;

                        base.Update(_folder);
                    }
                    else
                    {
                        throw new Exception("Folder not found");
                    }
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
    }
}
