using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chaining.Tests
{
    [TestClass]
    public class KeyProviderTests
    {
        [TestMethod]
        public void KeyProvider_Key_ReturnsKey()
        {
            var provider = new KeyProvider();

            var key = provider.Key();
        }

        [TestMethod]
        public void KeyProvider_CallingKeyTwice_ReturnsNewKeyEachTime()
        {
            var provider = new KeyProvider();

            var key1 = provider.Key();
            var key2 = provider.Key();

            Assert.AreNotEqual(key1, key2);
        }

        [TestMethod]
        public void KeyProvider_InvalidKey_ReturnsInvalidKey()
        {
            var provider = new KeyProvider();

            var key = provider.InvalidKey();

            Assert.IsFalse(provider.IsValid(key));
        }
    }
}
