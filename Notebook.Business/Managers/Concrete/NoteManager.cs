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
    public class NoteManager : Manager<Note>, INoteManager
    {
        private INoteDal noteDal;
        private IUserDal userDal;
        private IFolderDal folderDal;
        private IGroupDal groupDal;
        public NoteManager(INoteDal _noteDal, IUserDal _userDal, IFolderDal _folderDal, IGroupDal _groupDal) : base(_noteDal)
        {
            noteDal = _noteDal;
            userDal = _userDal;
            folderDal = _folderDal;
            groupDal = _groupDal;
        }

        [Validate(typeof(Note), typeof(NoteFluentValidation))]
        public override void Add(Note note)
        {
            if (!string.IsNullOrEmpty(note.GroupID))
                note.Group = groupDal.getOne(a => a.ID == note.GroupID);

            if (!string.IsNullOrEmpty(note.FolderID))
                note.Folder = folderDal.getOne(a => a.ID == note.FolderID);

            note.CreateDate = DateTime.Now;

            base.Add(note);
        }

        public void Delete(string NoteID, string UserID)
        {
            var note = noteDal.getOne(a => a.ID == NoteID && a.UserID == UserID);

            if (note != null)
                base.Delete(note);
            else
                throw new Exception("Note not found");
        }

        public NoteInfoModel GetNoteInfo(string NoteID = "", string UserID = "")
        {
            NoteInfoModel detail = null;

            var _note = noteDal.getMany(a => a.ID == NoteID)
                .Include(a => a.Group)
                .Include(a => a.Folder)
                    .ThenInclude(b => b.Group)
                .Include(a => a.Users)
                    .ThenInclude(b => b.User)
                .FirstOrDefault();

            if (_note != null)
            {
                detail = new NoteInfoModel();
                detail.ID = _note.ID;
                detail.Title = _note.Title;
                detail.Explanation = _note.Explanation;
                detail.Content = _note.Content;
                detail.CreateDate = _note.CreateDate;
                detail.UpdateDate = _note.UpdateDate;
                detail.Visible = _note.Visible;
                detail.ReadCount = _note.ReadCount;
                detail.Folder = _note.Folder;
                detail.Group = _note.Group != null ? new GroupInfoModel { ID = _note.GroupID } : null;
                detail.User = _note.Users.FirstOrDefault(a => a.Status == Status.Owner).User;

                var _user = _note.Users.FirstOrDefault(a => a.UserID == UserID);
                detail.Status = (_user != null) ? _user.Status : (!string.IsNullOrEmpty(UserID)) ? Status.User : Status.Visitor;

                UpdateNoteReadCount(_note);
            }

            return detail;
        }

        [Validate(typeof(Note), typeof(NoteFluentValidation))]
        public override void Update(Note model)
        {
            var _note = noteDal.getOne(a => a.ID == model.ID);
            if (_note != null && _note.UserID == model.UserID)
            {
                // TODO: AutoMapper Uygulanacak
                _note.Title = model.Title;
                _note.Explanation = model.Explanation;
                _note.Visible = model.Visible;
                _note.Tags = model.Tags;
                _note.Content = model.Content;
                _note.UpdateDate = DateTime.Now;

                base.Update(_note);
            }
            else
            {
                throw new Exception("Note not found");
            }
        }

        public void UpdateNoteReadCount(Note note)
        {
            note.ReadCount++;

            base.Update(note);
        }
    }
}
