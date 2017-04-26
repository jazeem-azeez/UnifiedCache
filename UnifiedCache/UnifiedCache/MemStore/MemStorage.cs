using UnifiedCache.UnifiedCache;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace UnifiedCache.MemStore
{
    /// <summary>
    /// MemoryCache <see cref="MemoryCache"/> Based IStorage Implementation : Beware that this uses Distributed Memory Concept
    /// Needs A Proper BackPlane for working
    /// </summary>
    /// <seealso cref="UnifiedCache.UnifiedCache.IStorage" />
    internal class MemStorage : IStorage
    {
        /// <summary>
        /// Initializes the <see cref="MemStorage"/> class.
        /// </summary>
        static MemStorage()
        {
            BackPlane = new MemCacheRedisBackPlane();
            BackPlane.StartSubscribe();
        }
        /// <summary>
        /// The back plane
        /// </summary>
        static readonly IPubSubBackPlane BackPlane;
        /// <summary>
        /// Deletes the specified keys.Direct Usage is not Recommended unless you know what your are wishing for
        /// </summary>
        /// <param name="keys">The keys.</param>
        public void Delete(List<string> keys)
        {
            foreach (var item in keys)
                MemoryCache.Default.Remove(item);
        }

        /// <summary>
        /// Deletes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Delete(string key)
        {
            MemoryCache.Default.Remove(key);
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
            return MemoryCache.Default[key] as T;
        }

        /// <summary>
        /// Invalidates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Invalidate(string key)
        {
            BackPlane.Publish(key);
        }

        /// <summary>
        /// Saves the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool Save(string key, string value) => Save<string>(key, value);

        /// <summary>
        /// Saves the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expiry">The expiry.</param>
        /// <returns></returns>
        public bool Save(string key, string value, TimeSpan expiry) => Save<string>(key, value, expiry);

        /// <summary>
        /// Saves the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool Save<T>(string key, T value) where T : class
        {
            if (value != null)
            {
                MemoryCache.Default[key] = value;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Saves the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="timeSpan">The time span.</param>
        /// <returns></returns>
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