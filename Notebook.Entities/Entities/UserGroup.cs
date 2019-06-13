using Notebook.Core.EntityRepository.Entities;
using Notebook.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notebook.Entities.Entities
{
    public class UserGroup : IEntity
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), MaxLength(8)]
        public string ID { get; set; }
        public bool Notification { get; set; }
        public Member MemberType { get; set; }
        public string GroupID { get; set; }
        public string UserID { get; set; }
        public DateTime CreateDate { get; set; }

        [ForeignKey("GroupID")]
        public virtual Group Group { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }
}
