using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;

namespace Notebook.Business.Managers.Abstract
{
    public interface IUserManager : IManager<User>
    {
        void LastActiveDateUpdate(User user);
        User Login(User user);
        User Cookie(string key);
    }
}
