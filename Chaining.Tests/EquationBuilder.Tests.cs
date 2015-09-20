using System;
using System.Linq;
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
        public void ToImmutableTree_ForNotUsedBuilder_ReturnsTreeWithRoot()
        {
            IEquationBuilder builder = new EquationBuilder();

            var tree = (builder as EquationBuilder).ToImmutableTree();

            Assert.IsTrue(tree.KeyProvider.IsValid(tree.GetRoot()));
        }

        [TestMethod]
        public void ToImmutableTree_WithOneValue_ReturnsTreeWithRootAndChild()
        {
            IEquationBuilder builder = new EquationBuilder();
            builder = builder.Value(42);

            var tree = (builder as EquationBuilder).ToImmutableTree();

            var children = tree.GetChildren(tree.GetRoot());
            Assert.AreEqual(1, children.Count());
        }

        [TestMethod]
        public void ToImmutableTree_WithTwoValues_ReturnsTreeWithRootAndTwoChildren()
        {
            IEquationBuilder builder = new EquationBuilder();
            builder = builder
                .Value(42)
                .Value(84);

            var tree = (builder as EquationBuilder).ToImmutableTree();

            var children = tree.GetChildren(tree.GetRoot());
            Assert.AreEqual(2, children.Count());
        }

        [TestMethod]
        public void ToImmutableTree_WithLiteral_ReturnsTreeWithRootAndChild()
        {
            IEquationBuilder builder = new EquationBuilder();
            builder = builder.Literal("x");

            var tree = (builder as EquationBuilder).ToImmutableTree();

            var children = tree.GetChildren(tree.GetRoot());
            Assert.AreEqual(1, children.Count());
        }

        [TestMethod]
        public void ToImmutableTree_WithAddAndTwoValues_ReturnsTreeWithRootAndChildren()
        {
            IEquationBuilder builder = new EquationBuilder();
            builder = builder
                .Value(1)
                .Add()
                .Value(2);

            var tree = (builder as EquationBuilder).ToImmutableTree();

            var children = tree.GetChildren(tree.GetRoot());
            Assert.AreEqual(3, children.Count());
        }

        [TestMethod]
        public void ToImmutableTree_WithAdd_ReturnsTreeWithAddOperator()
        {
            IEquationBuilder builder = new EquationBuilder();
            builder = builder.Add();

            var tree = (builder as EquationBuilder).ToImmutableTree();

            var children = tree.GetChildren(tree.GetRoot());
            var add = children.First();
            Assert.AreEqual("+", tree.ValueOf(add));
        }

        [TestMethod]
        public void ToImmutableTree_WithDivide_ReturnsTreeWithDivideOperator()
        {
            IEquationBuilder builder = new EquationBuilder();
            builder = builder.Divide();

            var tree = (builder as EquationBuilder).ToImmutableTree();

            var children = tree.GetChildren(tree.GetRoot());
            var divide = children.First();
            Assert.AreEqual("/", tree.ValueOf(divide));
        }

        [TestMethod]
        public void ToImmutableTree_WithDivideWithEmptyExpression_ReturnsTreeWithDivideOperator()
        {
            IEquationBuilder builder = new EquationBuilder();
            builder = builder.Divide(b => { });

            var tree = (builder as EquationBuilder).ToImmutableTree();

            var children = tree.GetChildren(tree.GetRoot());
            var divide = children.First();
            Assert.AreEqual("/", tree.ValueOf(divide));
        }

        [TestMethod]
        public void ToImmutableTree_WithDivideWithValueExpression_ReturnsTreeWithDivideOperatorAndValue()
        {
            IEquationBuilder builder = new EquationBuilder();
            builder = builder.Divide(b => b.Value(3));

            var tree = (builder as EquationBuilder).ToImmutableTree();

            var divide = tree.GetChildren(tree.GetRoot()).First();
            var value = tree.GetChildren(divide).First();
            Assert.AreEqual("/", tree.ValueOf(divide));
            Assert.AreEqual("3", tree.ValueOf(value));
        }

        private class TestIdentifier : IIdentifier
        {

        }

        private class TestItem : IItem
        {

        }
    }
}
