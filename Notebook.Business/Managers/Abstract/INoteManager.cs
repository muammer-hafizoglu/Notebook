using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;

namespace Notebook.Business.Managers.Abstract
{
    public interface INoteManager : IManager<Note>
    {
        void Delete(string NoteID, string UserID);
        void UpdateNoteReadCount(Note note);
    }
}
