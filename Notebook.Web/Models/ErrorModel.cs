using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notebook.Web.Models
{
    public class ErrorModel
    {
        public string Source { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
