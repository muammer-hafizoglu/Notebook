using Notebook.Business.Managers.Abstract;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notebook.Business.Managers.Concrete
{
    public class FolderNoteManager : Manager<FolderNote>, IFolderNoteManager
    {
        private IFolderNoteDal folderNoteDal;
        private INoteDal noteDal;
        private IFolderDal folderDal;
        public FolderNoteManager(IFolderNoteDal _folderNoteDal, INoteDal _noteDal, IFolderDal _folderDal) : base(_folderNoteDal)
        {
            folderNoteDal = _folderNoteDal;
            noteDal = _noteDal;
            folderDal = _folderDal;
        }

        public void Add(string NoteID = "", string FolderID = "")
        {
            var note = noteDal.getOne(a => a.ID == NoteID);
            var folder = folderDal.getOne(a => a.ID == FolderID);

            if (note != null && folder != null)
            {
                base.Add(new FolderNote
                {
                    Note = note,
                    Folder = folder,
                    CreateDate = DateTime.Now
                });
            }
            else
            {
                throw new Exception("Folder or note not found");
            }
        }
    }
}
