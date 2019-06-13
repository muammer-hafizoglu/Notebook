using Notebook.Core.CrossCuttingConcerns.Logging;
using SimpleProxy.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notebook.Core.Aspects.SimpleProxy.Logging
{
    public class LogAttribute : MethodInterceptionAttribute
    {
        internal readonly Type _loggerServiceType;
        internal readonly LogType _logType;
        public LogAttribute(Type loggerServiceType, LogType logType)
        {
            _loggerServiceType = loggerServiceType;
            _logType = logType;
        }
    }
}
