using System;
using System.Collections.Generic;

namespace UnifiedCache.Lib.UnifiedCache
{
    public interface IUnifiedCache
    {
        void Delete(string key);

        void Delete(List<string> keys);

        T Get<T>(string key) where T : class;

        string Get(string key);

        T GetorSet<T>(string key, Func<T> getItemCallback) where T : class;

        void Invalidate(string key);

        void Set<T>(string key, T data) where T : class;
    }
}