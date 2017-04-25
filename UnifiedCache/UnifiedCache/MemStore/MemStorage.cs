using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using UnifiedCache.Lib.UnifiedCache;

namespace UnifiedCache.Lib.MemStore
{
    internal class MemStorage : IStorage
    {
        static MemStorage()
        {
            BackPlane = new MemCacheRedisBackPlane();
            BackPlane.StartSubscribe();
        }

        private static IPubSubBackPlane BackPlane;

        public void Delete(List<string> keys)
        {
            foreach (var item in keys)
                MemoryCache.Default.Remove(item);
        }

        public void Delete(string key)
        {
            MemoryCache.Default.Remove(key);
        }

        public string Get(string key) => Get<string>(key);

        public T Get<T>(string key) where T : class
        {
            return MemoryCache.Default[key] as T;
        }

        public void Invalidate(string key)
        {
            BackPlane.Publish(key);
        }

        public bool Save(string key, string value) => Save<string>(key, value);

        public bool Save(string key, string value, TimeSpan expiry) => Save<string>(key, value, expiry);

        public bool Save<T>(string key, T value) where T : class
        {
            if (value != null)
            {
                MemoryCache.Default[key] = value;

                return true;
            }
            return false;
        }

        public bool Save<T>(string key, T value, TimeSpan timeSpan) where T : class
        {
            if (value != null)
            {
                MemoryCache.Default.Add(key, value, new DateTimeOffset(DateTime.UtcNow, timeSpan));
                return true;
            }
            return false;
        }
    }
}