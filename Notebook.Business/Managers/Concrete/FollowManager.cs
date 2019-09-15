using Microsoft.EntityFrameworkCore;
using Notebook.Business.Managers.Abstract;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.Entities.Entities;
using Notebook.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Notebook.Business.Managers.Concrete
{
    public class FollowManager : Manager<Follow>, IFollowManager
    {
        private IFollowDal servisDal;
        private IUserDal userDal;
        public FollowManager(IFollowDal _servisDal, IUserDal _userDal) : base(_servisDal)
        {
            servisDal = _servisDal;
            userDal = _userDal;
        }

        public void Follow(string FollowingID, string FollowerID = "")
        {
            var follow = base.getOne(a => a.Following.Username == FollowingID && a.Follower.Username == FollowerID);

            if (follow == null)
            {
                var follower = userDal.getOne(a => a.Username == FollowerID);
                var following = userDal.getOne(a => a.Username == FollowingID);

                if (follower != null && following != null)
                {
                    base.Add(new Follow
                    {
                        Follower = follower,
                        Following = following,
                        Notification = false,
                        Status = following.Lock ? Status.Wait : Status.Follow
                    });
                }
            }
        }

        public void Unfollow(string FollowingID, string FollowerID = "")
        {
            var follow = base.getOne(a => a.Following.Username == FollowingID && a.Follower.Username == FollowerID);

            if (follow != null)
            {
                base.Delete(follow);
            }
        }

        public void Delete(string FollowID, string UserID = "")
        {
            var follow = base.getOne(a => a.ID == FollowID && (a.Follower.Username == UserID || a.Following.Username == UserID));

            if (follow != null)
            {
                base.Delete(follow);
            }
        }
    }
}
