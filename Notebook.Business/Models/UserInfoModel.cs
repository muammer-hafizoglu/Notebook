using Notebook.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notebook.Business.Models
{
    public class UserInfoModel
    {
        public string ID { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public DateTime CreateDate { get; set; }
        public string Avatar { get; set; }
        public int GroupCount { get; set; }
        public int FolderCount { get; set; }
        public int NoteCount { get; set; }
        public int FollowerCount { get; set; }
        public int FollowingCount { get; set; }
        public int WaitingUserCount { get; set; }
        public bool Lock { get; set; }
        public Status Status { get; set; }
    }
}
