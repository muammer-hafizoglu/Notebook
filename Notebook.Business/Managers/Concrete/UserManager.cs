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
using Notebook.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Notebook.Business.Managers.Concrete
{
    public class UserManager : Manager<User>, IUserManager
    {
        private IUserDal servisDal;
        private ISettingsManager settingsManager;
        public UserManager(IUserDal _servisDal, ISettingsManager _settingsManager) : base(_servisDal)
        {
            servisDal = _servisDal;
            settingsManager = _settingsManager;
        }

        public UserInfoModel GetUserInfo(string ID, string UserID = "")
        {
            UserInfoModel user = null;

            var _user = servisDal.getMany(a => a.Username == ID)
                .Include(a => a.Groups)
                .Include(a => a.Notes)
                .Include(a => a.Follower)
                    .ThenInclude(b => b.Follower)
                .Include(a => a.Following)
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
                user.FollowerCount = _user.Follower.Where(a => a.Status == Status.Follow).Count();
                user.FollowingCount = _user.Following.Count;
                user.WaitingUserCount = _user.Follower.Where(a => a.Status == Status.Wait).Count();

                var follow = _user.Follower.FirstOrDefault(a => a.FollowerID == UserID);

                user.Status = (user.ID == UserID) ? Status.Owner : follow != null ? follow.Status :
                    !string.IsNullOrEmpty(UserID) ? Status.User : Status.Visitor;
            }

            return user;
        }

        [Validate(typeof(User),typeof(UserFluentValidation))]
        public override void Add(User model)
        {
            //var validation = new UserFluentValidation();
            //validation.Validate(model);

            var settings = settingsManager.GetSettings();

            EmailControl(model);

            if (!string.IsNullOrEmpty(model.Username))
                UsernameControl(model);
            else
                model.Username = CreatUsername();

            model.Avatar = "/notebook/images/avatar.png";
            model.Password = model.Password.SHA256Encrypt();
            model.CreateDate = DateTime.Now;
            model.LastActiveDate = DateTime.Now;
            model.CanUploadFile = false;
            model.TotalFileSize = settings?.TotalFileSize;
            model.SingleFileSize = settings?.SingleFileSize;

            base.Add(model);
        }

        [Validate(typeof(User), typeof(UserFluentValidation))]
        public override void Update(User model)
        {
            EmailControl(model);
            UsernameControl(model);

            var _user = servisDal.getMany(a => a.ID == model.ID).FirstOrDefault();
            if (_user != null)
            {
                _user.Email = model.Email;
                _user.Username = model.Username;
                _user.Name = model.Name;
                _user.Info = model.Info;
                _user.Password = model.Password;
                _user.Lock = model.Lock;
                _user.CanUploadFile = model.CanUploadFile;
                _user.SingleFileSize = model.SingleFileSize;
                _user.TotalFileSize = model.TotalFileSize;
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

        private string CreatUsername()
        {
            Random random = new Random();

            string username = "user" + random.Next(111111, 999999).ToString();

            while (servisDal.getOne(a => a.Username == username) != null)
            {
                username = "user" + random.Next(1111, 9999).ToString();
            }

            return username;
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

        public async Task<User> CookieAsync(string key)
        {
            User user = null;

            if (!string.IsNullOrEmpty(key))
            {
                user = await servisDal.getMany(a => a.Email == key).Include(a => a.Role).FirstOrDefaultAsync();
                if (user != null)
                {
                    LastActiveDateUpdate(user);
                }
            }

            return user;
        }

        
    }
}
