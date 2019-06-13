using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notebook.Web.Models
{
    public class DatatableParameters
    {
        public int draw { get; set; } = 1;
        public int start { get; set; } = 0;
        public int length { get; set; } = 10;
    }
}
