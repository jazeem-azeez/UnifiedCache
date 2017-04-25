using System;
using System.Collections.Generic;

namespace UnifiedCache.Lib.UnifiedCache
{
    public class UnifiedCache : IUnifiedCache
    {
        public UnifiedCache(IStorage store)
        {
            this.Store = store;
        }

        private IStorage Store { get; set; }

        public void Delete(string key)
        {
            Store.Delete(key);
        }

        public void Delete(List<string> keys)
        {
            Store.Delete(keys);
        }

        public string Get(string key) => Get<string>(key);

        public T Get<T>(string key) where T : class
        {
            return Store.Get<T>(key);
        }

        public T GetorSet<T>(string key, Func<T> getItemCallback) where T : class
        {
            T item = Get<T>(key);

            if (item == null)
            {
                item = getItemCallback();

                if (item != null)
                {
                    Set(key, item);
                }
            }

            return item;
        }

        public void Invalidate(string key)
        {
            Store.Invalidate(key);
        }

        public void Set<T>(string key, T item) where T : class
        {
            Store.Save(key, item);
        }

        public void Set<T>(string key, T item, TimeSpan expiry) where T : class
        {
            Store.Save(key, item, expiry);
        }
    }
}