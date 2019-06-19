using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Notebook.Business.Managers.Abstract
{
    public interface IUserNoteManager : IManager<UserNote>
    {
        IQueryable<UserNote> getUserNotes(string ID, bool IsActiveUser);
    }
}
