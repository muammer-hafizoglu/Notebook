using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notebook.Web.Models
{
    public class ParametersModel
    {
        public string Page { get; set; } = "1";
        public string Show { get; set; }
        public string Filter { get; set; }
        public string Value { get; set; }
        public string Order { get; set; }
        public string State { get; set; }
        public string Date { get; set; }
        public string Date2 { get; set; }
    }
}
