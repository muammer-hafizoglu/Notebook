using Notebook.Core.EntityRepository.Entities;
using Notebook.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notebook.Entities.Entities
{
    public class UserFolder : IEntity
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), MaxLength(8)]
        public string ID { get; set; }
        public bool Notification { get; set; }
        public Member MemberType { get; set; }
        public DateTime CreateDate { get; set; }
        public string FolderID { get; set; }
        public string UserID { get; set; }
        public virtual Folder Folder { get; set; }
        public virtual User User { get; set; }
    }
}
