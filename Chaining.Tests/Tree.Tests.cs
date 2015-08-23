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
            var tree = new Tree<int>();
        }

        [TestMethod]
        public void Tree_Contructor_WorksWithClassType()
        {
            var tree = new Tree<string>();
        }

        [TestMethod]
        public void Tree_Contructor_CreatesDefaultKeyProvider()
        {
            var tree = new Tree<string>();

            Assert.IsNotNull(tree.KeyProvider);
        }

        [TestMethod]
        public void Tree_ContructorWithKeyProvider_WorksWithValueType()
        {
            var keyProvider = new KeyProvider();

            var tree = new Tree<int>(keyProvider);
        }

        [TestMethod]
        public void Tree_ContructorWithKeyProvider_WorksWithClassType()
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
        public void Tree_AddRootWithKey_ReturnsAKey()
        {
            var tree = NewTree<string>();

            KeyType key;
            tree.AddRoot("root", out key);
        }

        [TestMethod]
        public void Tree_AddNodeWithKey_ReturnsAKey()
        {
            var keyProvider = new KeyProvider();
            var tree = new Tree<string>(keyProvider);
            KeyType rootKey;
            tree = tree.AddRoot("root", out rootKey);

            KeyType key;
            tree = tree.AddNode("value", rootKey, out key);
        }

        [TestMethod]
        public void Tree_AddNodeWithKey_ReturnsANewTree()
        {
            var keyProvider = new KeyProvider();
            var tree = new Tree<string>(keyProvider);
            KeyType rootKey;
            tree = tree.AddRoot("root", out rootKey);

            KeyType key;
            var newTree = tree.AddNode("value", rootKey, out key);

            Assert.AreNotEqual(tree, newTree);
        }

        [TestMethod]
        public void Tree_AddingTwoValues_ReturnsTwoDifferentKeys()
        {
            var keyProvider = new KeyProvider();
            var tree = new Tree<string>(keyProvider);
            KeyType rootKey;
            tree = tree.AddRoot("root", out rootKey);

            KeyType key1;
            KeyType key2;
            tree = tree.AddNode("value1", rootKey, out key1);
            tree = tree.AddNode("value2", rootKey, out key2);

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
            KeyType rootKey;
            tree = tree.AddRoot("root", out rootKey);

            var value = "value";
            KeyType key;
            tree = tree.AddNode(value, rootKey, out key);

            var returnedValue = tree.ValueOf(key);

            Assert.AreEqual(value, returnedValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Tree_AddNodeWithInvalidParentKey_Fails()
        {
            var tree = NewTree<string>();

            tree.AddNode("child", tree.KeyProvider.InvalidKey());
        }

        [TestMethod]
        public void Tree_GetParentOfAChild_ReturnsTheParent()
        {
            var tree = NewTree<string>();
            KeyType rootKey;
            KeyType parentKey;
            KeyType childKey;
            tree = tree.AddRoot("root", out rootKey);
            tree = tree.AddNode("parent", rootKey, out parentKey);
            tree = tree.AddNode("child", parentKey, out childKey);

            var foundParentKey = tree.GetParent(childKey);

            Assert.AreEqual(parentKey, foundParentKey);
        }

        [TestMethod]
        public void Tree_GetRootWithoutAddRootFirst_ReturnsInvalidKey()
        {
            var tree = NewTree<string>();

            var rootKey = tree.GetRoot();

            Assert.AreEqual(tree.KeyProvider.InvalidKey(), rootKey);
        }

        [TestMethod]
        public void Tree_GetRootAfterAddRootWithKeyFirst_ReturnsRootKey()
        {
            var tree = NewTree<string>();
            KeyType rootKey;
            tree = tree.AddRoot("root", out rootKey);

            var returnedRootKey = tree.GetRoot();

            Assert.AreEqual(rootKey, returnedRootKey);
        }

        [TestMethod]
        public void Tree_GetChildrenOfRootWithNoChild_ReturnsEmptyChildList()
        {
            var tree = NewTree<string>();
            KeyType rootKey;
            tree.AddRoot("root", out rootKey);

            var childen = tree.GetChildren(rootKey);

            Assert.AreEqual(0, childen.Count());
        }

        [TestMethod]
        public void Tree_GetChildrenOfRootWithSingleChild_ReturnsTheChildInAList()
        {
            var tree = NewTree<string>();
            KeyType rootKey, childKey;
            tree = tree.AddRoot("root", out rootKey);
            tree = tree.AddNode("child", rootKey, out childKey);

            var childen = tree.GetChildren(rootKey);

            Assert.AreEqual(1, childen.Count());
            Assert.AreEqual(childKey, childen.ElementAt(0));
        }

        [TestMethod]
        public void Tree_GetChildrenOfRootWithTwoChildren_ReturnsTheChildrenInAList()
        {
            var tree = NewTree<string>();
            KeyType rootKey;
            tree = tree.AddRoot("root", out rootKey);
            tree = tree.AddNode("child1", rootKey);
            tree = tree.AddNode("child2", rootKey);

            var childen = tree.GetChildren(rootKey);

            Assert.AreEqual(2, childen.Count());
        }

        [TestMethod]
        public void Tree_GetChildrenOfNodeWithSingleChild_ReturnsTheChildInAList()
        {
            var tree = NewTree<string>();
            KeyType rootKey, nodeKey, childKey;
            tree = tree.AddRoot("root", out rootKey);
            tree = tree.AddNode("node", rootKey, out nodeKey);
            tree = tree.AddNode("child", nodeKey, out childKey);

            var childen = tree.GetChildren(nodeKey);

            Assert.AreEqual(1, childen.Count());
            Assert.AreEqual(childKey, childen.ElementAt(0));
        }


        private Tree<T> NewTree<T>()
        {
            var keyProvider = new KeyProvider();
            var tree = new Tree<T>(keyProvider);
            return tree;
        }
    }
}
