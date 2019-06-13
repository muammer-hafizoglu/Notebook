using System;
using System.Collections.Generic;
using System.Text;

namespace Notebook.Core.CrossCuttingConcerns.Logging
{
    public class LogDetail
    {
        public LogDetail()
        {
            Arguments = new List<LogArgument>();
        }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string MethodName { get; set; }
        public string IPAddress { get; set; }
        public string Type { get; set; }
        public string Info { get; set; }
        public DateTime DateTime { get; set; }
        public List<LogArgument> Arguments { get; set; }
    }
}
