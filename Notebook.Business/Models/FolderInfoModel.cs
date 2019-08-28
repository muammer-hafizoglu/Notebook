using Notebook.Entities.Entities;
using Notebook.Entities.Enums;
using System;

namespace Notebook.Business.Models
{
    public class FolderInfoModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Explanation { get; set; }
        public DateTime CreateDate { get; set; }
        public Group Group { get; set; }
        public Visible Visible { get; set; }
        public int NoteCount { get; set; }
        public Status Status { get; set; }
    }
}
