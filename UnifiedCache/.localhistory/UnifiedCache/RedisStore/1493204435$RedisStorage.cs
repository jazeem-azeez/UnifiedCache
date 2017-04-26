using CareStack.Cache.UnifiedCache;
using CareStack.Cache.Utility.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace CareStack.Cache.RedisStore
{
    /// <summary>
    /// Redis Based Cache Storage Implementation
    /// </summary>
    /// <seealso cref="CareStack.Cache.UnifiedCache.IStorage" />
    internal class RedisStorage : IStorage
    {
        /// <summary>
        /// </summary>
        private readonly IDatabase DB;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisStorage"/> class.
        /// </summary>
        public RedisStorage()
        {
            this.DB = RedisFactory.Connection?.GetDatabase();
        }


        /// <summary>
        /// Deletes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Delete(string key)
        {
            if (IsConnected())
            {
                DB.KeyDelete(key);
            }
        }

        /// <summary>
        /// Deletes the specified keys.
        /// </summary>
        /// <param name="keys">The keys.</param>
        public void Delete(List<string> keys)
        {
            if (IsConnected())
            {
                var redisKeys = new List<RedisKey>();
                keys.ForEach(x =>
                {
                    RedisKey y = x;
                    redisKeys.Add(y);
                });
                DB.KeyDelete(redisKeys.ToArray());
            }
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string Get(string key)
        {

            if (IsConnected())
            {
                return DB.StringGet(key);
            }
            return null;
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            if (!IsConnected())
            {
                return default(T);
            }

            string strObj = DB.StringGet(key);
            return string.IsNullOrEmpty(strObj) ? default(T) : DeSerializeFromString<T>(strObj);
        }


        /// <summary>
        /// Determines whether this instance is connected.
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return this.DB != null && RedisFactory.Connection.IsConnected;
        }

        /// <summary>
        /// Saves the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <param name="expiry">The expiry.</param>
        /// <returns></returns>
        public bool Save<T>(string key, T item, TimeSpan expiry) where T : class
        {
            if (!IsConnected())
            {
                return false;
            }

            var objString = SerializeToString(item);

            return Save(key, objString, expiry);
        }

        /// <summary>
        /// Saves the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool Save(string key, string value)
        {
            return DB.StringSet(key, value);
        }

        /// <summary>
        /// Saves the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expiry">The expiry.</param>
        /// <returns></returns>
        public bool Save(string key, string value, TimeSpan expiry)
        {
            return DB.StringSet(key, value, expiry);
        }

        /// <summary>
        /// Saves the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool Save<T>(string key, T value) where T : class
        {
            if (!IsConnected())
            {
                return false;
            }
            var objString = SerializeToString(value);
            return Save(key, objString);
        }

        /// <summary>
        /// Des the serialize from string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strObj">The string object.</param>
        /// <returns></returns>
        private static T DeSerializeFromString<T>(string strObj) => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(strObj);

        /// <summary>
        /// Serializes to string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        private static string SerializeToString<T>(T obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Invalidates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Invalidate(string key)
        {
            Delete(key);
        }
    }
}