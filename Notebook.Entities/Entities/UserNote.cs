using Notebook.Core.EntityRepository.Entities;
using Notebook.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notebook.Entities.Entities
{
    public class UserNote : IEntity
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), MaxLength(8)]
        public string ID { get; set; }
        public DateTime CreateDate { get; set; }
        public Status Status { get; set; }
        public string NoteID { get; set; }
        public string UserID { get; set; }
        public virtual Note Note { get; set; }
        public virtual User User { get; set; }
    }
}
