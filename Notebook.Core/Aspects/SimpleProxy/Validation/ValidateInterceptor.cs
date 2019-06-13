using Microsoft.Extensions.DependencyInjection;
using Notebook.Core.CrossCuttingConcerns.Validation.Abstract;
using SimpleProxy;
using SimpleProxy.Interfaces;
using System;

namespace Notebook.Core.Aspects.SimpleProxy.Validation
{
    public class ValidateInterceptor : IMethodInterceptor
    {
        private ValidateAttribute _validateAttribute;
        private readonly IServiceProvider _serviceProvider;
        public ValidateInterceptor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void BeforeInvoke(InvocationContext invocationContext)
        {
            this._validateAttribute = invocationContext.GetAttributeFromMethod<ValidateAttribute>();

            if (typeof(IValidation).IsAssignableFrom(_validateAttribute._validatorType) == false)
            {
                throw new Exception("Wrong validation manager");
            }

            var validator = (IValidation)ActivatorUtilities.CreateInstance(_serviceProvider, _validateAttribute._validatorType);
            
            int entityPosition = invocationContext.GetParameterPosition(_validateAttribute._entityType);
            var entity = invocationContext.GetParameterValue(entityPosition);

            if (entity != null)
            {
                validator.Validator(entity);
            }
        }
        public void AfterInvoke(InvocationContext invocationContext, object methodResult)
        {
           // throw new NotImplementedException();
        }
    }
}
