using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Notebook.Core.EntityRepository.Entities;
using Notebook.Entities.Enums;

namespace Notebook.Entities.Entities
{
    public class Note : IEntity
    {
        public Note()
        {
            Users = new HashSet<UserNote>();
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), MaxLength(8)]
        public string ID { get; set; }
        public string Title { get; set; }
        public string Explanation { get; set; }
        public string Content { get; set; }
        public Visible Visible { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int ReadCount { get; set; }
        public string Tags { get; set; }
        public bool OpenToCopy { get; set; }
        public bool OpenToComments { get; set; }
        public int CopyCount { get; set; }
        public string UserID { get; set; }

        public string GroupID { get; set; }
        public virtual Group Group { get; set; }
        public string FolderID { get; set; }
        public virtual Folder Folder { get; set; }
        public virtual ICollection<UserNote> Users { get; set; }
    }
}
