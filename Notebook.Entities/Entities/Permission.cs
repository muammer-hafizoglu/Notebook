using Notebook.Core.EntityRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Notebook.Entities.Entities
{
    public class Permission : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), MaxLength(8)]
        public string ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsModule { get; set; }
    }
}
