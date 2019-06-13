using Notebook.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notebook.Web.Models
{
    public class NoteDetailModel
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Explanation { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public Visible Visible { get; set; }
        public string OwnerID { get; set; }
        public string OwnerName { get; set; }
        public int UserCount { get; set; }
        public int ReadCount { get; set; }
        public string List { get; set; }
    }
}
