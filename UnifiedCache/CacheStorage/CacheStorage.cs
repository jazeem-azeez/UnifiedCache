using System;
using System.Collections.Generic;

namespace CacheStorage
{
    public class CacheStorage
    {
        private IStorage Store { get; set; }

        public CacheStorage(IStorage store)
        {
            this.Store = store;
        }

        public void Delete(string key)
        {
            Store.Delete(key);
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

        public void Delete(List<string> keys)
        {
            Store.Delete(keys);
        }

        public T Get<T>(string key) where T : class
        {
            return Store.Get<T>(key);
        }

        public void Set<T>(string key, T item) where T : class
        {
            Store.Save<T>(key, item, new TimeSpan(1, 0, 0));
        }

        public void Set<T>(string key, T item, TimeSpan expiry) where T : class
        {
            Store.Save<T>(key, item, expiry);
        }
    }
}