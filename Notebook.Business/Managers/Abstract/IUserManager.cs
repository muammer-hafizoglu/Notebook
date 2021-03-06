﻿using Notebook.Business.Models;
using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notebook.Business.Managers.Abstract
{
    public interface IUserManager : IManager<User>
    {
        void LastActiveDateUpdate(User user);
        User Login(User user);
        User Cookie(string key);
        Task<User> CookieAsync(string key);
        UserInfoModel GetUserInfo(string ID, string UserID);
    }
}
