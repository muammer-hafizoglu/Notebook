using Notebook.Entities.Entities;
using Notebook.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notebook.Business.Models
{
    public class NoteInfoModel
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Explanation { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public Visible Visible { get; set; }
        public Status Status { get; set; }
        public int FavoriteCount { get; set; }
        public int ReadCount { get; set; }

        public User User { get; set; }
        public GroupInfoModel Group { get; set; }
        public Folder Folder { get; set; }
    }
}
