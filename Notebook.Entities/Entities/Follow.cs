using Notebook.Core.EntityRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Notebook.Entities.Entities
{
    public class Follow : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), MaxLength(8)]
        public string ID { get; set; }
        public bool Notification { get; set; }

        public string FollowerID { get; set; }
        public string FollowingID { get; set; }
        public virtual User Follower { get; set; }
        public virtual User Following { get; set; }
    }
}
