using Notebook.Core.EntityRepository.Entities;
using Notebook.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notebook.Entities.Entities
{
    public class Folder : IEntity
    {
        public Folder()
        {
            Notes = new HashSet<Note>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), MaxLength(8)]
        public string ID { get; set; }
        public string Name { get; set; }
        public string Explanation { get; set; }
        public Visible Visible { get; set; }
        public DateTime CreateDate { get; set; }

        public string GroupID { get; set; }
        public virtual Group Group { get; set; }

        public virtual ICollection<Note> Notes { get; set; }
    }
}
