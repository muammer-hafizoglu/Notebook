using System;
using System.Collections.Generic;
using System.Text;

namespace Notebook.Core.CrossCuttingConcerns.Validaton
{
    public class ValidateException : Exception
    {
        public ValidateException()
        {
            propertyExceptions = new HashSet<PropertyException>();
        }
        public ICollection<PropertyException> propertyExceptions { get; set; }
    }
}
