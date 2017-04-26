using System;
using System.Collections.Generic;

namespace UnifiedCache.UnifiedCache
{
    /// <summary>
    /// Defines IStorage Interface for Implementing Storages For cache
    /// </summary>
    public interface IStorage
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
        /// Saves the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <param name="timeSpan">The time span.</param>
        /// <returns></returns>
        bool Save<T>(string key, T item, TimeSpan timeSpan) where T : class;
        /// <summary>
        /// Saves the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        bool Save(string key, string value);

        /// <summary>
        /// Saves the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expiry">The expiry.</param>
        /// <returns></returns>
        bool Save(string key, string value, TimeSpan expiry);

        /// <summary>
        /// Saves the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        bool Save<T>(string key, T value) where T : class;
        /// <summary>
        /// Invalidates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        void Invalidate(string key);
    }
}