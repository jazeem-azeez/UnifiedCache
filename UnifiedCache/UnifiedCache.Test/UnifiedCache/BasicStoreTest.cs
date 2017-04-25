using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnifiedCache.Lib.UnifiedCache.Tests
{
    [TestClass()]
    public class BasicStoreTest
    {
        [TestMethod()]
        public void GetorSetTest()
        {
            var cache = UnifiedCacheFactory.GetUnifiedCache(UnifiedCacheType.MemCache);
            cache.GetorSet("Key", MockMethod);
            var result = cache.Get<string>("Key");
            Console.WriteLine(result);
            Assert.AreEqual("Value", result, "Reading From cache resulted in improper value");
        }

        public static string MockMethod() => "Value";
    }
}