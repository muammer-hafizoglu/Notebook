using Microsoft.Extensions.DependencyInjection;
using Notebook.Core.CrossCuttingConcerns.Caching;
using Notebook.Core.CrossCuttingConcerns.Logging;
using SimpleProxy;
using SimpleProxy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Notebook.Core.Aspects.SimpleProxy.Logging
{
    public class LogInterceptor : IMethodInterceptor
    {
        private LogAttribute _logAttribute;
        private readonly IServiceProvider _serviceProvider;
        private ILoggerService logManager;
        public LogInterceptor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void BeforeInvoke(InvocationContext invocationContext)
        {
            this._logAttribute = invocationContext.GetAttributeFromMethod<LogAttribute>();

            if (typeof(ILoggerService).IsAssignableFrom(_logAttribute._loggerServiceType) == false)
            {
                throw new Exception("Wrong log manager");
            }

            logManager = (ILoggerService)ActivatorUtilities.CreateInstance(_serviceProvider, _logAttribute._loggerServiceType);

            LogDetail _log = new LogDetail()
            {
                DateTime = DateTime.Now,
                FullName = invocationContext.GetInvocation().TargetType.FullName,
                MethodName = invocationContext.GetExecutingMethodInfo().Name,
                Type = _logAttribute._logType.ToString()
               // Arguments = GetArguments(invocationContext)
            };

            logManager.Logging(_log);
        }

        private List<LogArgument> GetArguments(InvocationContext invocationContext)
        {
            var arguments = new List<LogArgument>();

            int _argumentsLength = invocationContext.GetInvocation().Arguments.Length;

            if (_argumentsLength > 0)
            {
                for (int i = 0; i < _argumentsLength; i++)
                {
                    arguments.Add(new LogArgument
                    {
                        Name = invocationContext.GetParameterValue(i).ToString()

                    });
                }
            }

            return arguments;
        }

        public void AfterInvoke(InvocationContext invocationContext, object methodResult)
        {
           
        }
    }
}
