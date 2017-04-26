using CareStack.Cache.MemStore;
using CareStack.Cache.RedisStore;
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace CareStack.Cache.UnifiedCache
{
    /// <summary>
    /// This servers as the factory method that handles instantiate of cache instances
    /// </summary>
    public class UnifiedCacheFactory
    {
        /// <summary>
        /// Gets the unified cache.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static IUnifiedCache GetUnifiedCache(UnifiedCacheStorageTypes type)
        {
            switch (type)
            {
                case UnifiedCacheStorageTypes.RedisCache:
                    {
                        return GetNewRedisCacheInstance();
                    }
                case UnifiedCacheStorageTypes.MemCache:
                    {
                        return GetNewMemCacheInstance();
                    }
                case UnifiedCacheStorageTypes.FromConfig:
                    {
                        return GetNewCacheInstanceFromConfig();
                    }
            }
            return null;
        }

        /// <summary>
        /// Gets the new cache instance from configuration.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">Unknown Default Cache Type in UnifiedCacheFactorySettings </exception>
        private static IUnifiedCache GetNewCacheInstanceFromConfig()
        {
            var section = ConfigurationManager.GetSection("UnifiedCacheFactorySettings") as NameValueCollection;
            var value = section["DefaultCacheType"];
            switch (value.ToLower())
            {
                case "RedisCache":
                    return GetNewRedisCacheInstance();

                case "MemCache":
                    return GetNewMemCacheInstance();

                default: throw new Exception("Unknown Default Cache Type in UnifiedCacheFactorySettings ");
            }
        }

        /// <summary>
        /// Gets the new memory cache instance.
        /// </summary>
        /// <returns></returns>
        private static IUnifiedCache GetNewMemCacheInstance() => new UnifiedCache(new MemStorage());

        /// <summary>
        /// Gets the new redis cache instance.
        /// </summary>
        /// <returns></returns>
        private static IUnifiedCache GetNewRedisCacheInstance() => new UnifiedCache(new RedisStorage());
    }
}