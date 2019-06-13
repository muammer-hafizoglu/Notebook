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
            Groups = new HashSet<GroupNote>();
            Folders = new HashSet<FolderNote>();
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
        public string OwnerID { get; set; }
        public virtual User Owner { get; set; }
        public virtual ICollection<UserNote> Users { get; set; }
        public virtual ICollection<GroupNote> Groups { get; set; }
        public virtual ICollection<FolderNote> Folders { get; set; }
    }
}
