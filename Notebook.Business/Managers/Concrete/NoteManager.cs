using Notebook.Business.Managers.Abstract;
using Notebook.Business.Tools.Validation.FluentValidation;
using Notebook.Core.Aspects.SimpleProxy.Validation;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;
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
            var user = userDal.getOne(a => a.ID == note.OwnerID);
            if (user != null)
            {
                note.Owner = user;
                note.CreateDate = DateTime.Now;

                if (!string.IsNullOrEmpty(note.GroupID))
                    note.Group = groupDal.getOne(a => a.ID == note.GroupID);

                if (!string.IsNullOrEmpty(note.FolderID))
                    note.Folder = folderDal.getOne(a => a.ID == note.FolderID);

                base.Add(note);
            }
            else
            {
                throw new Exception("User not found");
            }
        }

        public void Delete(string NoteID, string UserID)
        {
            var note = noteDal.getOne(a => a.ID == NoteID && a.OwnerID == UserID);

            if (note != null)
                base.Delete(note);
            else
                throw new Exception("Note not found");
        }

        [Validate(typeof(Note), typeof(NoteFluentValidation))]
        public override void Update(Note model)
        {
            var _note = noteDal.getOne(a => a.ID == model.ID);
            if (_note != null && _note.OwnerID == model.OwnerID)
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
