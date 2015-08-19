using System;
using System.Linq;
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
        public void Tree_AddNode_ReturnsAKey()
        {
            var keyProvider = new KeyProvider();
            var tree = new Tree<string>(keyProvider);

            var key = tree.AddNode("value");

            Assert.IsTrue(key is KeyType);
        }

        [TestMethod]
        public void Tree_AddingTwoValues_ReturnsTwoDifferentKeys()
        {
            var keyProvider = new KeyProvider();
            var tree = new Tree<string>(keyProvider);

            var key1 = tree.AddNode("value1");
            var key2 = tree.AddNode("value2");

            Assert.AreNotEqual(key1, key2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Tree_ValueWithInvalidKey_Fails()
        {
            var keyProvider = new KeyProvider();
            var tree = new Tree<string>(keyProvider);

            var invalidKey = -1;
            tree.Value(invalidKey);
        }

        [TestMethod]
        public void Tree_ValueWithValidKey_ReturnsThePreviouslyAddedValue()
        {
            var keyProvider = new KeyProvider();
            var tree = new Tree<string>(keyProvider);

            var value = "value";
            var key = tree.AddNode(value);

            var returnedValue = tree.Value(key);

            Assert.AreEqual(value, returnedValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Tree_SetParentWithInvalidParentKey_Fails()
        {
            var tree = NewTree<string>();

            var childKey = tree.AddNode("child");
            var parentKey = InvalidKey();

            tree.SetParent(childKey, parentKey);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Tree_SetParentWithInvalidChildKey_Fails()
        {
            var tree = NewTree<string>();

            var parentKey = tree.AddNode("parent");
            var childKey = InvalidKey();

            tree.SetParent(childKey, parentKey);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Tree_GetParentOfAChildWithNoParent_Fails()
        {
            var tree = NewTree<string>();

            var childKey = tree.AddNode("child");

            tree.GetParent(childKey);
        }

        [TestMethod]
        public void Tree_GetParentOfAChildWithParent_ReturnsTheParent()
        {
            var tree = NewTree<string>();
            var childKey = tree.AddNode("child");
            var parentKey = tree.AddNode("parent");

            tree.SetParent(childKey, parentKey);

            var foundParentKey = tree.GetParent(childKey);

            Assert.AreEqual(parentKey, foundParentKey);
        }

        [TestMethod]
        public void Tree_GetRootWithoutAddRootFirst_ReturnsInvalidKey()
        {
            var tree = NewTree<string>();

            var rootKey = tree.GetRoot();

            Assert.AreEqual(InvalidKey(), rootKey);
        }

        [TestMethod]
        public void Tree_GetRootAfterAddRootFirst_ReturnsRootKey()
        {
            var tree = NewTree<string>();
            var rootKey = tree.AddRoot("root");

            var returnedRootKey = tree.GetRoot();

            Assert.AreEqual(rootKey, returnedRootKey);
        }

        [TestMethod]
        public void Tree_GetChildrenOfRootWithNoChild_ReturnsEmptyChildList()
        {
            var tree = NewTree<string>();
            var rootKey = tree.AddRoot("root");

            var childen = tree.GetChildren(rootKey);

            Assert.AreEqual(0, childen.Count());
        }

        [TestMethod]
        public void Tree_GetChildrenOfRootWithSingleChild_ReturnsTheChildInAList()
        {
            var tree = NewTree<string>();
            var rootKey = tree.AddRoot("root");
            var childKey = tree.AddNode("child");
            tree.SetParent(childKey, rootKey);

            var childen = tree.GetChildren(rootKey);

            Assert.AreEqual(1, childen.Count());
            Assert.AreEqual(childKey, childen.ElementAt(0));
        }


        private KeyType InvalidKey()
        {
            return -1;
        }

        private Tree<T> NewTree<T>()
        {
            var keyProvider = new KeyProvider();
            var tree = new Tree<T>(keyProvider);
            return tree;
        }
    }
}
