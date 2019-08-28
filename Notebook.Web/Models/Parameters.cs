using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notebook.Web.Models
{
    public class Parameters
    {
        public string ID { get; set; }
        public string Page { get; set; } = "1";
        public string Show { get; set; }
        public string Search { get; set; }
        public string Filter { get; set; }
        public string Order { get; set; }
        public string State { get; set; }
        public string List { get; set; }
        public string Date { get; set; }
        public string Date2 { get; set; }
    }
}
