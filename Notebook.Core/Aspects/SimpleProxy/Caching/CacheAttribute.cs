using SimpleProxy.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notebook.Core.Aspects.SimpleProxy.Caching
{
    public class CacheAttribute : MethodInterceptionAttribute
    {
        internal readonly Type _cacheType;
        internal readonly int _cacheByMinute;
        public CacheAttribute(Type cacheType, int cacheByMinute = 60)
        {
            _cacheType = cacheType;
            _cacheByMinute = cacheByMinute;
        }
    }
}
