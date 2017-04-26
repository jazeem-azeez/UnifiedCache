using System;
using System.Collections.Generic;

namespace UnifiedCache.UnifiedCache
{
    /// <summary>
    ///  This class handles the Caching Implementation For IUnifiedCache , User use various storage by implementing IStorage
    /// </summary>
    /// <seealso cref="UnifiedCache.UnifiedCache.IUnifiedCache" />
    public class UnifiedCache : IUnifiedCache
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnifiedCache"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        public UnifiedCache(IStorage store)
        {
            this.Store = store;
        }

        /// <summary>
        /// Gets or sets the store.
        /// </summary>
        /// <value>
        /// The store.
        /// </value>
        private IStorage Store { get; set; }

        /// <summary>
        /// Deletes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Delete(string key)
        {
            Store.Delete(key);
        }

        /// <summary>
        /// Deletes the specified keys.
        /// </summary>
        /// <param name="keys">The keys.</param>
        public void Delete(List<string> keys)
        {
            Store.Delete(keys);
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string Get(string key) => Get<string>(key);

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            return Store.Get<T>(key);
        }

        /// <summary>
        /// Getors the set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="getItemCallback">The get item callback.</param>
        /// <returns></returns>
        public T GetorSet<T>(string key, Func<T> getItemCallback) where T : class
        {
            var item = Get<T>(key);

            if (item == null)
            {
#pragma warning disable CC0031 // Check for null before calling a delegate
                item = getItemCallback();
#pragma warning restore CC0031 // Check for null before calling a delegate

                if (item != null)
                {
                    Set(key, item);
                }
            }

            return item;
        }


        /// <summary>
        /// Invalidates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Invalidate(string key)
        {
            Store.Invalidate(key);
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        public void Set<T>(string key, T item) where T : class
        {
            Store.Save(key, item);
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <param name="expiry">The expiry.</param>
        public void Set<T>(string key, T item, TimeSpan expiry) where T : class
        {
            Store.Save(key, item, expiry);
        }
    }
}