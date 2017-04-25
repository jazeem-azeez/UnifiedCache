using System;
using System.Collections.Generic;

namespace CacheStorage
{
    public interface IStorage
    {
        void Delete(string key);
        T Get<T>(string key) where T : class;
        void Save<T>(string key, T item, TimeSpan timeSpan) where T : class;
        void Delete(List<string> keys);
    }
}