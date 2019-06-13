using Microsoft.Extensions.Caching.Memory;
using System;

namespace Notebook.Core.CrossCuttingConcerns.Caching.MemoryCache
{
    public class MemoryCacheManager : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;
        public MemoryCacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public void Add(string key, object data, int cacheByMinute)
        {
            if (data != null)
            {
                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions();
                cacheExpirationOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(cacheByMinute);
                
                _memoryCache.Set(key, data, cacheExpirationOptions);
            }
        }

        public void Delete(string key)
        {
            _memoryCache.Remove(key);
        }

        public object Get(string key)
        {
            return _memoryCache.Get(key);
        }

        public bool IsAdd(string key)
        {
            return _memoryCache.Get(key) != null ? true : false;
        }

        public bool TryGet(object key, out object result)
        {
            return _memoryCache.TryGetValue(key, out result);
        }
    }
}
