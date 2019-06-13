using Notebook.Core.EntityRepository.Entities;
using Notebook.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notebook.Entities.Entities
{
    public class Folder : IEntity
    {
        public Folder()
        {
            Users = new HashSet<UserFolder>();
            Groups = new HashSet<GroupFolder>();
            Notes = new HashSet<FolderNote>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), MaxLength(8)]
        public string ID { get; set; }
        public string Name { get; set; }
        public string Explanation { get; set; }
        public Visible Visible { get; set; }
        public DateTime CreateDate { get; set; }
        public string OwnerID { get; set; }
        public virtual User Owner { get; set; }
        public virtual ICollection<UserFolder> Users { get; set; }
        public virtual ICollection<FolderNote> Notes { get; set; }
        public virtual ICollection<GroupFolder> Groups { get; set; }
    }
}
