using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KeyType = System.Int32;

namespace Chaining.Tests
{
    [TestClass]
    public class TreeTests
    {
        [TestMethod]
        public void Tree_Contructor_WorksWithValueType()
        {
            var keyProvider = new KeyProvider();

            var tree = new Tree<int>(keyProvider);
        }

        [TestMethod]
        public void Tree_Contructor_WorksWithClassType()
        {
            var keyProvider = new KeyProvider();

            var tree = new Tree<string>(keyProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Tree_ConstructorWithNullKeyProvider_Fails()
        {
            var tree = new Tree<string>(null);
        }

        [TestMethod]
        public void Tree_Add_ReturnsAKey()
        {
            var keyProvider = new KeyProvider();
            var tree = new Tree<string>(keyProvider);

            var key = tree.Add("value");

            Assert.IsTrue(key is KeyType);
        }

        [TestMethod]
        public void Tree_AddingTwoValues_ReturnsTwoDifferentKeys()
        {
            var keyProvider = new KeyProvider();
            var tree = new Tree<string>(keyProvider);

            var key1 = tree.Add("value1");
            var key2 = tree.Add("value2");

            Assert.AreNotEqual(key1, key2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Tree_GetWithInvalidKey_Fails()
        {
            var keyProvider = new KeyProvider();
            var tree = new Tree<string>(keyProvider);

            var invalidKey = -1;
            tree.Get(invalidKey);
        }

        [TestMethod]
        public void Tree_GetWithValidKey_ReturnsThePreviouslyAddedValue()
        {
            var keyProvider = new KeyProvider();
            var tree = new Tree<string>(keyProvider);

            var value = "value";
            var key = tree.Add(value);

            var returnedValue = tree.Get(key);

            Assert.AreEqual(value, returnedValue);
        }
    }
}
