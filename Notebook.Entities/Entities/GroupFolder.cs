using Notebook.Core.EntityRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notebook.Entities.Entities
{
    public class GroupFolder : IEntity
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), MaxLength(8)]
        public string ID { get; set; }
        public DateTime CreateDate { get; set; }
        public string FolderID { get; set; }
        public string GroupID { get; set; }

        [ForeignKey("FolderID")]
        public virtual Folder Folder { get; set; }
        [ForeignKey("GroupID")]
        public virtual Group Group { get; set; }
    }
}
