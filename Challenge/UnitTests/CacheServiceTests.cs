using Challenge.Service.CacheService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Challenge.UnitTests
{
    [TestClass]
    public class CacheServiceTests
    {
        [TestMethod]
        public void Set_And_TryGetValue_ReturnsTrueForCachedItem()
        {
            var cache = new CacheService();
            var from = "EUR";
            var to = "USD";
            var start = DateTime.Now.AddDays(-1);
            var end = DateTime.Now;
            var value = "test";

            cache.Set(from, to, start, end, value);

            var result = cache.TryGetValue(from, to, start, end, out var cachedValue);

            Assert.IsTrue(result);
            Assert.AreEqual(value, cachedValue);
        }

        [TestMethod]
        public void TryGetValue_ReturnsFalseForNonexistentItem()
        {
            var cache = new CacheService();
            var result = cache.TryGetValue("EUR", "USD", DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1), out var cachedValue);

            Assert.IsFalse(result);
            Assert.IsNull(cachedValue);
        }

        [TestMethod]
        public void Clear_RemovesAllItems()
        {
            var cache = new CacheService();
            cache.Set("EUR", "USD", DateTime.Now.AddDays(-1), DateTime.Now, "value");
            cache.Clear();

            var result = cache.TryGetValue("EUR", "USD", DateTime.Now.AddDays(-1), DateTime.Now, out var cachedValue);

            Assert.IsFalse(result);
        }
    }
}