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
            Notes = new HashSet<Note>();
            Folders = new HashSet<Folder>();
            Groups = new HashSet<Group>();
            SucscribedFolders = new HashSet<UserFolder>();
            SucscribedGroups = new HashSet<UserGroup>();
            SucscribedNotes = new HashSet<UserNote>();
            Followers = new HashSet<Follow>();
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
        public DateTime CreateDate { get; set; }
        public DateTime LastActiveDate { get; set; }
        public string Avatar { get; set; }

        public virtual Role Role { get; set; }
        public virtual UserSettings Settings { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Folder> Folders { get; set; }
        public virtual ICollection<Follow> Followers { get; set; }
        public virtual ICollection<Follow> Following { get; set; }
        public virtual ICollection<UserNote> SucscribedNotes { get; set; }
        public virtual ICollection<UserFolder> SucscribedFolders { get; set; }
        public virtual ICollection<UserGroup> SucscribedGroups { get; set; }
    }
}
