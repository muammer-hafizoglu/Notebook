using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notebook.Web.Models
{
    public class UserProfileModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public DateTime CreateDate { get; set; }
        public string Avatar { get; set; }
        public int GroupCount { get; set; }
        public int FolderCount { get; set; }
        public int NoteCount { get; set; }
        public int FollowerCount { get; set; }
        public int FollowingCount { get; set; }
        public string List { get; set; }

        public bool Follower { get; set; }
    }
}
