using Notebook.Core.EntityRepository.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notebook.Entities.Entities
{
    public class Settings : IEntity
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), MaxLength(8)]
        public string ID { get; set; }

        // General 
        public string WebAddress { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Metadata { get; set; }
        public string Logo { get; set; }
        public string Icon { get; set; }
        public string Introduction { get; set; }
        public string Footer { get; set; }
        public string DefaultLanguage { get; set; }

        // Mail
        public string Email { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsMailActive { get; set; }

        // Contact
        public string Gmail { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Linkedin { get; set; }

        // Membership
        public bool IsMembershipOpen { get; set; }
        public bool MembershipEmailControl { get; set; }

        // Limitations
        public string TotalFileSize { get; set; }
        public string SingleFileSize { get; set; }
    }
}
