using Notebook.Core.EntityRepository.Entities;
using Notebook.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Notebook.Entities.Entities
{
    public class Event : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), MaxLength(8)]
        public string ID { get; set; }
        public Operation Operation { get; set; }
        public Product Type { get; set; }
        public string Explation { get; set; }
        public bool View { get; set; }
        public DateTime CreateDate { get; set; }
        public string Url { get; set; }
        public string ProductName { get; set; }
        public string ProductID { get; set; }


        public User User { get; set; }
    }
}
