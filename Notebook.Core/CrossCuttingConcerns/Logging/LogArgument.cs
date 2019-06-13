using System;
using System.Collections.Generic;
using System.Text;

namespace Notebook.Core.CrossCuttingConcerns.Logging
{
    public class LogArgument
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public object Value { get; set; }
    }
}
