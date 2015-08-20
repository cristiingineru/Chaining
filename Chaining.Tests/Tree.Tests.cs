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
            var rootKey = tree.AddRoot("root");

            var key = tree.AddNode("value", rootKey);

            Assert.IsTrue(key is KeyType);
        }

        [TestMethod]
        public void Tree_AddingTwoValues_ReturnsTwoDifferentKeys()
        {
            var keyProvider = new KeyProvider();
            var tree = new Tree<string>(keyProvider);
            var rootKey = tree.AddRoot("root");

            var key1 = tree.AddNode("value1", rootKey);
            var key2 = tree.AddNode("value2", rootKey);

            Assert.AreNotEqual(key1, key2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Tree_ValueOfWithInvalidKey_Fails()
        {
            var keyProvider = new KeyProvider();
            var tree = new Tree<string>(keyProvider);

            var invalidKey = -1;
            tree.ValueOf(invalidKey);
        }

        [TestMethod]
        public void Tree_ValueOfWithValidKey_ReturnsThePreviouslyAddedValue()
        {
            var keyProvider = new KeyProvider();
            var tree = new Tree<string>(keyProvider);
            var rootKey = tree.AddRoot("root");

            var value = "value";
            var key = tree.AddNode(value, rootKey);

            var returnedValue = tree.ValueOf(key);

            Assert.AreEqual(value, returnedValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Tree_AddNodeWithInvalidParentKey_Fails()
        {
            var tree = NewTree<string>();

            tree.AddNode("child", InvalidKey());
        }

        [TestMethod]
        public void Tree_GetParentOfAChild_ReturnsTheParent()
        {
            var tree = NewTree<string>();
            var rootKey = tree.AddRoot("root");
            var parentKey = tree.AddNode("parent", rootKey);
            var childKey = tree.AddNode("child", parentKey);

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
            var childKey = tree.AddNode("child", rootKey);

            var childen = tree.GetChildren(rootKey);

            Assert.AreEqual(1, childen.Count());
            Assert.AreEqual(childKey, childen.ElementAt(0));
        }

        [TestMethod]
        public void Tree_GetChildrenOfRootWithTwoChildren_ReturnsTheChildrenInAList()
        {
            var tree = NewTree<string>();
            var rootKey = tree.AddRoot("root");
            var child1Key = tree.AddNode("child1", rootKey);
            var child2Key = tree.AddNode("child2", rootKey);

            var childen = tree.GetChildren(rootKey);

            Assert.AreEqual(2, childen.Count());
        }

        [TestMethod]
        public void Tree_GetChildrenOfNodeWithSingleChild_ReturnsTheChildInAList()
        {
            var tree = NewTree<string>();
            var rootKey = tree.AddRoot("root");
            var nodeKey = tree.AddNode("node", rootKey);
            var childKey = tree.AddNode("child", nodeKey);

            var childen = tree.GetChildren(nodeKey);

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
