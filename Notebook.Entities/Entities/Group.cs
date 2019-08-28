using Notebook.Core.EntityRepository.Entities;
using Notebook.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notebook.Entities.Entities
{
    public class Group : IEntity
    {
        public Group()
        {
            Users = new HashSet<UserGroup>();
            Notes = new HashSet<Note>();
            Folders = new HashSet<Folder>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), MaxLength(8)]
        public string ID { get; set; }
        public string Name { get; set; }
        public string Explanation { get; set; }
        public Visible Visible { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsRequiredApproval { get; set; }
        public string UserID { get; set; }
        public virtual ICollection<UserGroup> Users { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
        public virtual ICollection<Folder> Folders { get; set; }
    }
}
