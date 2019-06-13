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
            Notes = new HashSet<GroupNote>();
            Folders = new HashSet<GroupFolder>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), MaxLength(8)]
        public string ID { get; set; }
        public string Name { get; set; }
        public string Explanation { get; set; }
        public Visible Visible { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsRequiredApproval { get; set; }
        public string OwnerID { get; set; }
        public virtual User Owner { get; set; }
        public virtual ICollection<UserGroup> Users { get; set; }
        public virtual ICollection<GroupNote> Notes { get; set; }
        public virtual ICollection<GroupFolder> Folders { get; set; }
    }
}
