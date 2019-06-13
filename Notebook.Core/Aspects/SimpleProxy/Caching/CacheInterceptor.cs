using Microsoft.Extensions.DependencyInjection;
using Notebook.Core.CrossCuttingConcerns.Caching;
using SimpleProxy;
using SimpleProxy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notebook.Core.Aspects.SimpleProxy.Caching
{
    public class CacheInterceptor : IMethodInterceptor
    {
        private CacheAttribute _cacheAttribute;
        private readonly IServiceProvider _serviceProvider;
        private ICacheManager cacheManager;
        public CacheInterceptor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void BeforeInvoke(InvocationContext invocationContext)
        {
            this._cacheAttribute = invocationContext.GetAttributeFromMethod<CacheAttribute>();

            if (typeof(ICacheManager).IsAssignableFrom(_cacheAttribute._cacheType) == false)
            {
                throw new Exception("Wrong cache manager");
            }

            cacheManager = (ICacheManager)ActivatorUtilities.CreateInstance(_serviceProvider, _cacheAttribute._cacheType);

            string key = GetMethodKey(invocationContext);


            cacheManager.TryGet(key,out var result);
            if (result != null)
            {
                invocationContext.OverrideMethodReturnValue(result);
                invocationContext.BypassInvocation();
            }

        }
        public void AfterInvoke(InvocationContext invocationContext, object methodResult)
        {
            string key = GetMethodKey(invocationContext);

            if (this.cacheManager.IsAdd(key) == false)
            {
                this.cacheManager.Add(key, methodResult, _cacheAttribute._cacheByMinute);
            }
        }

        private string GetMethodKey(InvocationContext invocationContext)
        {
            var key = string.Format("{0}.{1}",
                invocationContext.GetInvocation().TargetType.FullName,
                invocationContext.GetInvocation().Method.Name);

            var arguments = invocationContext.GetInvocation().Arguments;

            key += "(" + string.Join(",", arguments) + ")";

            return key;
        }
    }
}
