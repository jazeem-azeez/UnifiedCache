using System;
using System.Collections.Generic;

namespace UnifiedCache.UnifiedCache
{
    /// <summary>
    ///This Defines IUnifiedCache , User use various storage by implementing IStorage
    /// </summary>
    public interface IUnifiedCache
    {

        /// <summary>
        /// Deletes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        void Delete(string key);

        /// <summary>
        /// Deletes the specified keys.
        /// </summary>
        /// <param name="keys">The keys.</param>
        void Delete(List<string> keys);

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        T Get<T>(string key) where T : class;
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        string Get(string key);

        /// <summary>
        /// Getors the set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="getItemCallback">The get item callback.</param>
        /// <returns></returns>
        T GetorSet<T>(string key, Func<T> getItemCallback) where T : class;

        /// <summary>
        /// Invalidates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        void Invalidate(string key);

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        void Set<T>(string key, T data) where T : class;
    }
}