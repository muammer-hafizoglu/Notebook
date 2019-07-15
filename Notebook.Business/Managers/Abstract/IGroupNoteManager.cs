using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;

namespace Notebook.Business.Managers.Abstract
{
    public interface IGroupNoteManager : IManager<GroupNote>
    {
        void Add(string NoteID, string GroupID);
    }
}
