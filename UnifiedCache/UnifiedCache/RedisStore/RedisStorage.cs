using StackExchange.Redis;
using System;
using System.Collections.Generic;
using UnifiedCache.Lib.UnifiedCache;

namespace UnifiedCache.Lib.RedisStore
{
    internal class RedisStorage : IStorage
    {
        private IDatabase DB = null;

        public RedisStorage()
        {
            this.DB = RedisFactory.Connection?.GetDatabase();
        }

        public void Delete(string key)
        {
            if (IsConnected())
            {
                DB.KeyDelete(key);
            }
        }

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

        public string Get(string key)
        {
            try
            {
                if (!IsConnected())
                {
                    return null;
                }
                return DB.StringGet(key);
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        public T Get<T>(string key) where T : class
        {
            if (!IsConnected())
            {
                return default(T);
            }

            string strObj = DB.StringGet(key);
            return string.IsNullOrEmpty(strObj) ? default(T) : DeSerializeFromString<T>(strObj);
        }

        public bool IsConnected()
        {
            return this.DB != null && RedisFactory.Connection.IsConnected;
        }

        public bool Save<T>(string key, T item, TimeSpan expiry) where T : class
        {
            if (!IsConnected())
            {
                return false;
            }

            var objString = SerializeToString(item);

            return Save(key, objString, expiry);
        }

        public bool Save(string key, string value)
        {
            return DB.StringSet(key, value);
        }

        public bool Save(string key, string value, TimeSpan expiry)
        {
            return DB.StringSet(key, value, expiry);
        }

        public bool Save<T>(string key, T value) where T : class
        {
            if (!IsConnected())
            {
                return false;
            }
            var objString = SerializeToString(value);
            return Save(key, objString);
        }

        private static T DeSerializeFromString<T>(string strObj) => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(strObj);

        private static string SerializeToString<T>(T obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public void Invalidate(string key)
        {
            Delete(key);
        }
    }
}