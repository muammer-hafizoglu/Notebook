using Notebook.Core.EntityRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notebook.Entities.Entities
{
    public class FolderNote : IEntity
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), MaxLength(8)]
        public string ID { get; set; }
        public DateTime CreateDate { get; set; }
        public string NoteID { get; set; }
        public string FolderID { get; set; }
        public virtual Folder Folder { get; set; }
        public virtual Note Note { get; set; }
    }
}
