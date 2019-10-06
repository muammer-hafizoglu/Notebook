using Notebook.Business.Models;
using Notebook.Entities.Entities;
using Notebook.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Notebook.Business.Managers.Abstract
{
    public interface IEventManager : IManager<Event>
    {
        IQueryable<Event> GetEventsForTimeline(string userID, Status status);
        void Delete(string ID);
        void Show(string ID);
        void Hide(string ID);
    }
}
