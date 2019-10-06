using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;

namespace Notebook.Business.Managers.Abstract
{
    public interface ISettingsManager : IManager<Settings>
    {
        Settings GetSettings();
    }
}
