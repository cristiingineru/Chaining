using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Chaining.Tests
{
    [TestClass]
    public class EquationBuilderTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateItem_WithNoFactory_Fails()
        {
            var id = new TestIdentifier();

            IEquationBuilder builder = new EquationBuilder();

            builder.CreateItem(id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateItemReturningItem_WithNoFactory_Fails()
        {
            var id = new TestIdentifier();

            IEquationBuilder builder = new EquationBuilder();

            IItem item;
            builder.CreateItem(id, out item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateItem_WithFactoryAndUnknownIdentifier_Fails()
        {
            var id = new TestIdentifier();
            var mock = new Mock<IFactory>();
            mock.Setup(o => o.CanCreate(id)).Returns(false);
            IEquationBuilder builder = new EquationBuilder(new[] { mock.Object });

            builder.CreateItem(id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateItemReturningItem_WithFactoryAndUnknownIdentifier_Fails()
        {
            var id = new TestIdentifier();
            var mock = new Mock<IFactory>();
            mock.Setup(o => o.CanCreate(id)).Returns(false);
            IEquationBuilder builder = new EquationBuilder(new[] { mock.Object });

            IItem item;
            builder.CreateItem(id, out item);
        }

        [TestMethod]
        public void CreateItemReturningItem_WithFactoryAndKnownIdentifier_ReturnsTheNewItem()
        {
            var id = new TestIdentifier();
            var item = new TestItem();
            var mock = new Mock<IFactory>();
            mock.Setup(o => o.CanCreate(id)).Returns(true);
            mock.Setup(o => o.Create(id)).Returns(item);
            IEquationBuilder builder = new EquationBuilder(new[] { mock.Object });

            IItem createItem;
            builder.CreateItem(id, out createItem);

            Assert.AreEqual(item, createItem);
        }

        [TestMethod]
        public void CreateItem_WithFactoryAndKnownIdentifier_ReturnsTheBuilder()
        {
            var id = new TestIdentifier();
            var item = new TestItem();
            var mock = new Mock<IFactory>();
            mock.Setup(o => o.CanCreate(id)).Returns(true);
            mock.Setup(o => o.Create(id)).Returns(item);
            IEquationBuilder builder = new EquationBuilder(new[] { mock.Object });

            var returnedBuilder = builder.CreateItem(id);

            Assert.AreEqual(builder, returnedBuilder);
        }

        [TestMethod]
        public void CreateItemReturningItem_WithFactoryAndKnownIdentifier_ReturnsTheBuilder()
        {
            var id = new TestIdentifier();
            var item = new TestItem();
            var mock = new Mock<IFactory>();
            mock.Setup(o => o.CanCreate(id)).Returns(true);
            mock.Setup(o => o.Create(id)).Returns(item);
            IEquationBuilder builder = new EquationBuilder(new[] { mock.Object });

            IItem createItem;
            var returnedBuilder = builder.CreateItem(id, out createItem);

            Assert.AreEqual(builder, returnedBuilder);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateItem_WithFactoriesThatCanCreateSameItem_Fails()
        {
            var id = new TestIdentifier();
            var item = new TestItem();
            var mock = new Mock<IFactory>();
            mock.Setup(o => o.CanCreate(id)).Returns(true);
            mock.Setup(o => o.Create(id)).Returns(item);
            IEquationBuilder builder = new EquationBuilder(new[] { mock.Object, mock.Object });

            builder.CreateItem(id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateItemReturningItem_WithFactoriesThatCanCreateSameItem_Fails()
        {
            var id = new TestIdentifier();
            var item = new TestItem();
            var mock = new Mock<IFactory>();
            mock.Setup(o => o.CanCreate(id)).Returns(true);
            mock.Setup(o => o.Create(id)).Returns(item);
            IEquationBuilder builder = new EquationBuilder(new[] { mock.Object, mock.Object });

            IItem createItem;
            builder.CreateItem(id, out createItem);
        }


        [TestMethod]
        public void ToImmutableTree_WithEmptyBuilder_ReturnsTreeWithRoot()
        {
            var builder = new EquationBuilder();

            var tree = builder.ToImmutableTree();

            Assert.IsTrue(tree.KeyProvider.IsValid(tree.GetRoot()));
        }


        private class TestIdentifier : IIdentifier
        {

        }

        private class TestItem : IItem
        {

        }
    }
}
