using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Notebook.Core.EntityRepository.Entities;

namespace Notebook.Entities.Entities
{
    public class Role : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), MaxLength(8)]
        public string ID { get; set; }
        public string Name { get; set; }
        public string Permissions { get; set; }

    }
}
