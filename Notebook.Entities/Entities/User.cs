using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Notebook.Core.EntityRepository.Entities;

namespace Notebook.Entities.Entities
{
    public class User : IEntity
    {
        public User()
        {
            Groups = new HashSet<UserGroup>();
            Notes = new HashSet<UserNote>();
            Follower = new HashSet<Follow>();
            Following = new HashSet<Follow>();
        }

        [Key,DatabaseGenerated(DatabaseGeneratedOption.None), MaxLength(8)]
        public string ID { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public bool Approve { get; set; }
        public bool Lock { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastActiveDate { get; set; }
        public string Avatar { get; set; }

        public virtual Role Role { get; set; }
        public virtual UserSettings Settings { get; set; }
        public virtual ICollection<Follow> Follower { get; set; }
        public virtual ICollection<Follow> Following { get; set; }
        public virtual ICollection<UserNote> Notes { get; set; }
        public virtual ICollection<UserGroup> Groups { get; set; }
    }
}
