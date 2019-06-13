using System;
using System.Collections.Generic;
using System.Text;

namespace Notebook.Core.CrossCuttingConcerns.Caching
{
    public interface ICacheManager
    {
        bool TryGet(object key, out object result);
        object Get(string key);
        void Add(string key, object data, int cacheByMinute);
        bool IsAdd(string key);
        void Delete(string key);
    }
}
