using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnifiedCache.UnifiedCache.Tests
{
    [TestClass()]
    public class BasicStoreTest
    {
        [TestMethod()]
        public void GetorSetTest()
        {
            MemCache();
            RedisTest();
        }

        private static void MemCache()
        {
            var cache = UnifiedCacheFactory.GetUnifiedCache(UnifiedCacheStorageTypes.MemCache);
            cache.GetorSet("Key", MockMethod);
            var result = cache.Get<string>("Key");
            Console.WriteLine(result);
            Assert.AreEqual("Value", result, "Reading From cache resulted in improper value");
        }

        private static void RedisTest()
        {
            var cache = UnifiedCacheFactory.GetUnifiedCache(UnifiedCacheStorageTypes.RedisCache);
            cache.GetorSet("Key", MockMethod);
            var result = cache.Get<string>("Key");
            Console.WriteLine(result);
            Assert.AreEqual("Value", result, "Reading From cache resulted in improper value");
        }

        public static string MockMethod() => "Value";
    }
}