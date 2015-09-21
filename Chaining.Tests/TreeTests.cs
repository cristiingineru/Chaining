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
            var tree = new Tree<string>();

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
            var tree = new Tree<string>();

            tree.AddNode("child", tree.KeyProvider.InvalidKey());
        }

        [TestMethod]
        public void Tree_GetParentOfAChild_ReturnsTheParent()
        {
            var tree = new Tree<string>();
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
            var tree = new Tree<string>();

            var rootKey = tree.GetRoot();

            Assert.AreEqual(tree.KeyProvider.InvalidKey(), rootKey);
        }

        [TestMethod]
        public void Tree_GetRootAfterAddRootWithKeyFirst_ReturnsRootKey()
        {
            var tree = new Tree<string>();
            KeyType rootKey;
            tree = tree.AddRoot("root", out rootKey);

            var returnedRootKey = tree.GetRoot();

            Assert.AreEqual(rootKey, returnedRootKey);
        }

        [TestMethod]
        public void Tree_GetChildrenOfRootWithNoChild_ReturnsEmptyChildList()
        {
            var tree = new Tree<string>();
            KeyType rootKey;
            tree.AddRoot("root", out rootKey);

            var childen = tree.GetChildren(rootKey);

            Assert.AreEqual(0, childen.Count());
        }

        [TestMethod]
        public void Tree_GetChildrenOfRootWithSingleChild_ReturnsTheChildInAList()
        {
            var tree = new Tree<string>();
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
            var tree = new Tree<string>();
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
            var tree = new Tree<string>();
            KeyType rootKey, nodeKey, childKey;
            tree = tree.AddRoot("root", out rootKey);
            tree = tree.AddNode("node", rootKey, out nodeKey);
            tree = tree.AddNode("child", nodeKey, out childKey);

            var childen = tree.GetChildren(nodeKey);

            Assert.AreEqual(1, childen.Count());
            Assert.AreEqual(childKey, childen.ElementAt(0));
        }

        [TestMethod]
        public void Tree_CopyBranchWithInvalidSourceTree_NoOp()
        {
            var tree = new Tree<string>();
            KeyType root;
            tree = tree.AddRoot(String.Empty, out root);

            Tree<string> sourceTree = null;
            var sourceBranch = tree.KeyProvider.InvalidKey();
            var changedTree = tree.CopyBranch(sourceTree, sourceBranch, root);

            Assert.AreEqual(tree, changedTree);
        }

        [TestMethod]
        public void Tree_CopyBranchWithInvalidSourceBranch_NoOp()
        {
            var tree = new Tree<string>();
            KeyType root;
            tree = tree.AddRoot(String.Empty, out root);

            var sourceTree = new Tree<string>();
            var sourceBranch = sourceTree.KeyProvider.InvalidKey();
            var changedTree = tree.CopyBranch(sourceTree, sourceBranch, root);

            Assert.AreEqual(tree, changedTree);
        }

        [TestMethod]
        public void Tree_CopyBranchWithSingleNodeSource_CopiesTheSource()
        {
            var tree = new Tree<string>();
            KeyType root;
            tree = tree.AddRoot(String.Empty, out root);

            var sourceTree = new Tree<string>();
            KeyType sourceBranch;
            var value = "x";
            sourceTree = sourceTree.AddRoot(value, out sourceBranch);
            tree = tree.CopyBranch(sourceTree, sourceBranch, root);

            Assert.AreEqual(value, tree.GetValueOfChild(0));
        }

        [TestMethod]
        public void Tree_CopyBranchWithTwoNodeSource_CopiesTheSource()
        {
            var tree = new Tree<string>();
            KeyType root;
            tree = tree.AddRoot(String.Empty, out root);

            var sourceTree = new Tree<string>();
            KeyType sourceBranch;
            var value1 = "x1";
            var value2 = "x2";
            sourceTree = sourceTree.AddRoot(value1, out sourceBranch);
            sourceTree = sourceTree.AddNode(value2, sourceBranch);
            tree = tree.CopyBranch(sourceTree, sourceBranch, root);

            Assert.AreEqual(value1, tree.GetValueOfChild(0));
            Assert.AreEqual(value2, tree.GetValueOfChild(0, 0));
        }

        [TestMethod]
        public void Tree_CopyBranchWithThreeNodeSource_CopiesTheSource()
        {
            var tree = new Tree<string>();
            KeyType root;
            tree = tree.AddRoot(String.Empty, out root);

            var sourceTree = new Tree<string>();
            KeyType sourceBranch;
            var value1 = "x1";
            var value2 = "x2";
            var value3 = "x3";
            sourceTree = sourceTree.AddRoot(value1, out sourceBranch);
            sourceTree = sourceTree.AddNode(value2, sourceBranch);
            sourceTree = sourceTree.AddNode(value3, sourceBranch);
            tree = tree.CopyBranch(sourceTree, sourceBranch, root);

            Assert.AreEqual(value1, tree.GetValueOfChild(0));
            Assert.AreEqual(value2, tree.GetValueOfChild(0, 0));
            Assert.AreEqual(value3, tree.GetValueOfChild(0, 1));
        }

        [TestMethod]
        public void Tree_CopyBranchWithThreeLevelsDeepSource_CopiesTheSource()
        {
            var tree = new Tree<string>();
            KeyType root;
            tree = tree.AddRoot(String.Empty, out root);

            var sourceTree = new Tree<string>();
            KeyType sourceBranch, l2;
            var value1 = "x1";
            var value2 = "x2";
            var value3 = "x3";
            sourceTree = sourceTree.AddRoot(value1, out sourceBranch);
            sourceTree = sourceTree.AddNode(value2, sourceBranch, out l2);
            sourceTree = sourceTree.AddNode(value3, l2);
            tree = tree.CopyBranch(sourceTree, sourceBranch, root);

            Assert.AreEqual(value1, tree.GetValueOfChild(0));
            Assert.AreEqual(value2, tree.GetValueOfChild(0, 0));
            Assert.AreEqual(value3, tree.GetValueOfChild(0, 0, 0));
        }
    }


    public static class TreeNavigation
    {
        public static T GetValueOfChild<T>(this Tree<T> tree, int l1)
        {
            return tree.ValueOf(
                tree.GetChildOfPath(l1));
        }

        public static T GetValueOfChild<T>(this Tree<T> tree, int l1, int l2)
        {
            return tree.ValueOf(
                tree.GetChildOfPath(l1, l2));
        }

        public static T GetValueOfChild<T>(this Tree<T> tree, int l1, int l2, int l3)
        {
            return tree.ValueOf(
                tree.GetChildOfPath(l1, l2, l3));
        }

        public static KeyType GetChildOfPath<T>(this Tree<T> tree, int l1)
        {
            return tree.GetChildren(tree.GetRoot()).ElementAt(l1);
        }

        public static KeyType GetChildOfPath<T>(this Tree<T> tree, int l1, int l2)
        {
            return tree.GetChildren(tree.GetChildren(tree.GetRoot()).ElementAt(l1)).ElementAt(l2);
        }

        public static KeyType GetChildOfPath<T>(this Tree<T> tree, int l1, int l2, int l3)
        {
            return tree.GetChildren(tree.GetChildren(tree.GetChildren(tree.GetRoot()).ElementAt(l1)).ElementAt(l2)).ElementAt(l3);
        }
    }
}
