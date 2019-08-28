using Microsoft.EntityFrameworkCore;
using Notebook.Business.Managers.Abstract;
using Notebook.Business.Models;
using Notebook.Business.Tools.Logging;
using Notebook.Business.Tools.Validation.FluentValidation;
using Notebook.Core.Aspects.SimpleProxy.Caching;
using Notebook.Core.Aspects.SimpleProxy.Logging;
using Notebook.Core.Aspects.SimpleProxy.Validation;
using Notebook.Core.CrossCuttingConcerns.Caching.MemoryCache;
using Notebook.Core.CrossCuttingConcerns.Logging;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Notebook.Business.Managers.Concrete
{
    public class UserManager : Manager<User>, IUserManager
    {
        private IUserDal servisDal;
        public UserManager(IUserDal _servisDal) : base(_servisDal)
        {
            servisDal = _servisDal;
        }

        [Validate(typeof(User),typeof(UserFluentValidation))]
        public override void Add(User model)
        {
            EmailControl(model);
            UsernameControl(model);

            model.Password = model.Password.SHA256Encrypt();
            model.CreateDate = DateTime.Now;
            model.LastActiveDate = DateTime.Now;

            base.Add(model);
        }

        [Validate(typeof(User), typeof(UserFluentValidation))]
        public override void Update(User model)
        {
            EmailControl(model);
            UsernameControl(model);

            var _user = servisDal.getMany(a => a.ID == model.ID).Include(a => a.Role).FirstOrDefault();
            if (_user != null)
            {
                _user.Email = model.Email;
                _user.Username = model.Username;
                _user.Name = model.Name;
                _user.Info = model.Info;
                _user.Password = model.Password;
                _user.Lock = model.Lock;
            }

            base.Update(_user);
        }

        private void EmailControl(User model)
        {
            var _user = servisDal.getOne(a => a.Email == model.Email);

            if (_user != null)
            {
                if (!string.IsNullOrEmpty(model.ID))
                {
                    if (_user.ID != model.ID)
                    {
                        throw new Exception("This email address is not available");
                    }
                }
                else
                {
                    throw new Exception("This email address is not available");
                }
            }
        }

        private void UsernameControl(User model)
        {
            if (!string.IsNullOrEmpty(model.Username))
            {
                var _user = servisDal.getOne(a => a.Username == model.Username);

                if (_user != null)
                {
                    if (!string.IsNullOrEmpty(model.ID))
                    {
                        if (_user.ID != model.ID)
                        {
                            throw new Exception("This username is not available");
                        }
                    }
                    else
                    {
                        throw new Exception("This username is not available");
                    }
                }
            }
        }

        //[Cache(typeof(MemoryCacheManager), 20)]
        //[Log(typeof(FileLogger),LogType.Info)]
        public override List<User> getAll(int Skip, int Take)
        {
            return base.getAll(Skip, Take);
        }

        //[Cache(typeof(MemoryCacheManager), 20)]
        public override IQueryable<User> getAll()
        {
            return base.getAll();
        }

        public void LastActiveDateUpdate(User user)
        {
            user.LastActiveDate = DateTime.Now;

            servisDal.Update(user);
        }

        public User Login(User user)
        {
            var _user = servisDal.getMany(a => (a.Username == user.Email || a.Email == user.Email) && a.Password == user.Password.SHA256Encrypt()).Include(a => a.Role).FirstOrDefault();
            if (_user != null)
            {
                if (_user.Approve)
                {
                    LastActiveDateUpdate(_user);

                    return _user;
                }
                else
                {
                    throw new Exception("Your account is not active");
                }
            }
            else
            {
                throw new Exception("Username or password is wrong");
            }
        }

        public User Cookie(string key)
        {
            User user = null;

            if (!string.IsNullOrEmpty(key))
            {
                user = servisDal.getMany(a => a.Email == key).Include(a => a.Role).FirstOrDefault();
                if (user != null)
                {
                    LastActiveDateUpdate(user);
                }
            }

            return user;
        }

        public UserInfoModel GetUserInfo(string ID)
        {
            UserInfoModel user = null;

            var _user = servisDal.getMany(a => a.Username == ID)
                .Include(a => a.Groups)
                .Include(a => a.Notes)
                .FirstOrDefault();

            if (_user != null)
            {
                user = new UserInfoModel();
                user.ID = _user.ID;
                user.Username = _user.Username;
                user.Name = _user.Name;
                user.Info = _user.Info;
                user.CreateDate = _user.CreateDate;
                user.Avatar = _user.Avatar;
                user.Lock = _user.Lock;
                user.GroupCount = _user.Groups.Count;
                user.NoteCount = _user.Notes.Count;
            }

            return user;
        }
    }
}
