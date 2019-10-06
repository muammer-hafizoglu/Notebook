using System;
using System.Collections.Generic;
using System.Text;

namespace Notebook.Business.Models
{
    public class MailInfoModel
    {
        public string Subject { get; set; }
        public string Message { get; set; }
        public string MailTo { get; set; }
        public string File { get; set; }
    }
}
