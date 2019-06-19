using Microsoft.EntityFrameworkCore;
using Notebook.Business.Managers.Abstract;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.Entities.Entities;
using Notebook.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Notebook.Business.Managers.Concrete
{
    public class UserNoteManager : Manager<UserNote>, IUserNoteManager
    {
        private IUserNoteDal servisDal;
        public UserNoteManager(IUserNoteDal _servisDal) : base(_servisDal)
        {
            servisDal = _servisDal;
        }

        public IQueryable<UserNote> getUserNotes(string ID, bool IsActiveUser)
        {
            return servisDal.getMany(a => a.UserID == ID && (!IsActiveUser ? a.Note.Visible == Visible.Public : true)).Include(a => a.Note).OrderByDescending(a => a.CreateDate);
        }
    }
}
