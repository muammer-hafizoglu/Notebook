using Notebook.Business.Managers.Abstract;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notebook.Business.Managers.Concrete
{
    public class GroupNoteManager : Manager<GroupNote>, IGroupNoteManager
    {
        private IGroupNoteDal groupNoteDal;
        private INoteDal noteDal;
        private IGroupDal groupDal;
        public GroupNoteManager(IGroupNoteDal _groupNoteDal, INoteDal _noteDal, IGroupDal _groupDal) : base(_groupNoteDal)
        {
            groupNoteDal = _groupNoteDal;
            noteDal = _noteDal;
            groupDal = _groupDal;
        }

        public void Add(string NoteID = "", string GroupID = "")
        {
            var note = noteDal.getOne(a => a.ID == NoteID);
            var group = groupDal.getOne(a => a.ID == GroupID);

            if (note != null && group != null)
            {
                base.Add(new GroupNote
                {
                    Note = note,
                    Group = group,
                    CreateDate = DateTime.Now
                });
            }
            else
            {
                throw new Exception("Group or note not found");
            }
        }
    }
}
