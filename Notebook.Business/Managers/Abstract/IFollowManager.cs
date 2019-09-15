using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;

namespace Notebook.Business.Managers.Abstract
{
    public interface IFollowManager : IManager<Follow>
    {
        void Follow(string FollowingID, string FollowerID);
        void Unfollow(string FollowingID, string FollowerID);
        void Delete(string FollowID, string UserID);
    }
}
