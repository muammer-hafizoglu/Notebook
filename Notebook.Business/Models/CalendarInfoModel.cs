using System;
using System.Collections.Generic;
using System.Text;

namespace Notebook.Business.Models
{
    public class CalendarInfoModel
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Location { get; set; }
        public string Start { get; set; }
        public string Finish { get; set; }
    }
}
