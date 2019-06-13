using SimpleProxy.Attributes;
using System;

namespace Notebook.Core.Aspects.SimpleProxy.Validation
{
    public class ValidateAttribute : MethodInterceptionAttribute
    {
        internal readonly Type _validatorType;
        internal readonly Type _entityType;
        public ValidateAttribute(Type entityType,Type validatorType)
        {
            _entityType = entityType;
            _validatorType = validatorType;
        }
    }
}
