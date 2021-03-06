﻿using Notebook.Entities.Entities;
using Notebook.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notebook.Business.Models
{
    public class GroupInfoModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Explanation { get; set; }
        public DateTime CreateDate { get; set; }
        public Visible Visible { get; set; }
        public int FolderCount { get; set; }
        public int NoteCount { get; set; }
        public int UserCount { get; set; }
        public int WaitingUser { get; set; }
        public Status Status { get; set; }

        public User User { get; set; }
    }
}
