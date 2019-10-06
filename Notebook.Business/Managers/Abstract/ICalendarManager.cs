using Notebook.Business.Models;
using Notebook.Entities.Entities;
using Notebook.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Notebook.Business.Managers.Abstract
{
    public interface ICalendarManager : IManager<Calendar>
    {
        void Delete(string ID);
    }
}
