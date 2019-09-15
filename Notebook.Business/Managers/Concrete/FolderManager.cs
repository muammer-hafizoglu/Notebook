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
                if (_member != null && (_member.Status == Status.Owner || _member.Status == Status.Manager))
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
            var _folder = servisDal.getMany(a => a.ID == FolderID)
                .Include(a => a.Notes)
                .Include(a => a.Group)
                    .ThenInclude(b => b.Users)
                .FirstOrDefault();
            if (_folder != null)
            {
                var _member = _folder.Group.Users.Where(a => a.UserID == UserID).FirstOrDefault();
                if (_member != null && (_member.Status == Status.Owner || _member.Status == Status.Manager))
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

        public FolderInfoModel GetFolderInfo(string FolderID, string UserID = "")
        {
            FolderInfoModel folder = null;

            var _folder = servisDal.getMany(a => a.ID == FolderID)
                .Include(a => a.Group)
                    .ThenInclude(b => b.Users)
                .Include(a => a.Notes)
                .FirstOrDefault();

            if (_folder != null)
            {
                folder = new FolderInfoModel();
                folder.ID = _folder.ID;
                folder.Name = _folder.Name;
                folder.Explanation = _folder.Explanation;
                folder.CreateDate = _folder.CreateDate;
                folder.Visible = _folder.Visible;
                folder.NoteCount = _folder.Notes.Count;
                folder.Group = _folder.Group;

                var user = _folder.Group.Users.FirstOrDefault(a => a.UserID == UserID);
                folder.Status = user != null ? user.Status : !string.IsNullOrEmpty(UserID) ? Status.User : Status.Visitor;
            }

            return folder;
        }

        [Validate(typeof(Folder), typeof(FolderFluentValidation))]
        public void Update(Folder model, string UserID)
        {
            var _group = groupDal.getMany(a => a.ID == model.Group.ID).Include(a => a.Users).FirstOrDefault();
            if (_group != null)
            {
                var _member = _group.Users.Where(a => a.UserID == UserID).FirstOrDefault();
                if (_member != null && (_member.Status == Status.Owner || _member.Status == Status.Manager))
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
