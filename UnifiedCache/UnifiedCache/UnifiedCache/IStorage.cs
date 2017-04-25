using System;
using System.Collections.Generic;

namespace UnifiedCache.Lib.UnifiedCache
{
    public interface IStorage
    {
        void Delete(string key);

        void Delete(List<string> keys);

        T Get<T>(string key) where T : class;

        string Get(string key);

        bool Save<T>(string key, T item, TimeSpan timeSpan) where T : class;

        bool Save(string key, string value);

        bool Save(string key, string value, TimeSpan expiry);

        bool Save<T>(string key, T value) where T : class;

        void Invalidate(string key);
    }
}