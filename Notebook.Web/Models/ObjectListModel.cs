using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notebook.Web.Models
{
    public class ObjectListModel
    {
        public object Datalist { get; set; }
        public int TotalData { get; set; }
        public int TotalPage { get; set; }
        public int ShowInPage { get; set; }
        public int ActivePage { get; set; }
        public string Pagination { get; set; }
        public string Title { get; set; }
    }
}
