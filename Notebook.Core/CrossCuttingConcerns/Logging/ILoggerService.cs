using System;
using System.Collections.Generic;
using System.Text;

namespace Notebook.Core.CrossCuttingConcerns.Logging
{
    public interface ILoggerService
    {
        void Logging(LogDetail log);
    }
}
