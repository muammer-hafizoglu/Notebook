using System;
using System.Collections.Generic;
using System.Text;

namespace Notebook.Core.CrossCuttingConcerns.Validaton
{
    public class PropertyException
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
        public string ResourceName { get; set; }
        public string ErrorCode { get; set; }
    }
}
