using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Immutable;

namespace Chaining.Tests
{
    [TestClass]
    public class ImmutableTests
    {
        [TestMethod]
        public void ImmitableArray()
        {
            var array = ImmutableArray<int>.Empty;

            var array2 = array.Add(10);

            Assert.AreNotEqual(array, array2);
        }

        [TestMethod]
        public void ImmutableDictionary()
        {
            var dictionary = ImmutableDictionary<int, string>.Empty;

            var dictionary2 = dictionary.Add(1, "a");

            Assert.AreNotEqual(dictionary, dictionary2);
        }
    }
}
