using System;
using System.Collections.Specialized;
using System.Configuration;
using UnifiedCache.Lib.MemStore;
using UnifiedCache.Lib.RedisStore;

namespace UnifiedCache.Lib.UnifiedCache
{
    public class UnifiedCacheFactory
    {
        public static IUnifiedCache GetUnifiedCache(UnifiedCacheType type)
        {
            switch (type)
            {
                case UnifiedCacheType.RedisCache:
                    {
                        return GetNewRedisCacheInstance();
                    }
                case UnifiedCacheType.MemCache:
                    {
                        return GetNewMemCacheInstance();
                    }
                case UnifiedCacheType.FromConfig:
                    {
                        return GetNewCacheInstanceFromConfig();
                    }
            }
            return null;
        }

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

        private static IUnifiedCache GetNewMemCacheInstance() => new UnifiedCache(new MemStorage());

        private static IUnifiedCache GetNewRedisCacheInstance() => new UnifiedCache(new RedisStorage());
    }
}