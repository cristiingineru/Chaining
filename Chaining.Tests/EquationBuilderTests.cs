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
        public void IsConformWithIEquationBuilderDynamicContract()
        {
            var builder = new EquationBuilder();

            Assert.IsTrue(IEquationBuilderDynamicContractChecker.IsConform(builder));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateItem_WithNoFactory_Fails()
        {
            var id = new Mock<IIdentifier>().Object;

            IEquationBuilder builder = new EquationBuilder();

            builder.CreateItem(id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateItemReturningItem_WithNoFactory_Fails()
        {
            var id = new Mock<IIdentifier>().Object;

            IEquationBuilder builder = new EquationBuilder();

            IItem item;
            builder.CreateItem(id, out item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateItem_WithFactoryAndUnknownIdentifier_Fails()
        {
            var id = new Mock<IIdentifier>().Object;
            var factory = new Mock<IFactory>();
            factory.Setup(o => o.CanCreate(id)).Returns(false);
            IEquationBuilder builder = new EquationBuilder(new[] { factory.Object });

            builder.CreateItem(id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateItemReturningItem_WithFactoryAndUnknownIdentifier_Fails()
        {
            var id = new Mock<IIdentifier>().Object;
            var factory = new Mock<IFactory>();
            factory.Setup(o => o.CanCreate(id)).Returns(false);
            IEquationBuilder builder = new EquationBuilder(new[] { factory.Object });

            IItem item;
            builder.CreateItem(id, out item);
        }

        [TestMethod]
        public void CreateItemReturningItem_WithFactoryAndKnownIdentifier_ReturnsTheNewItem()
        {
            var id = new Mock<IIdentifier>().Object;
            var item = new Mock<IItem>().Object;
            var factory = new Mock<IFactory>();
            factory.Setup(o => o.CanCreate(id)).Returns(true);
            factory.Setup(o => o.Create(id)).Returns(item);
            IEquationBuilder builder = new EquationBuilder(new[] { factory.Object });

            IItem createItem;
            builder.CreateItem(id, out createItem);

            Assert.AreEqual(item, createItem);
        }

        [TestMethod]
        public void CreateItem_WithFactoryAndKnownIdentifier_ReturnsTheBuilder()
        {
            var id = new Mock<IIdentifier>().Object;
            var item = new Mock<IItem>().Object;
            var factory = new Mock<IFactory>();
            factory.Setup(o => o.CanCreate(id)).Returns(true);
            factory.Setup(o => o.Create(id)).Returns(item);
            IEquationBuilder builder = new EquationBuilder(new[] { factory.Object });

            var returnedBuilder = builder.CreateItem(id);

            Assert.AreEqual(builder, returnedBuilder);
        }

        [TestMethod]
        public void CreateItemReturningItem_WithFactoryAndKnownIdentifier_ReturnsTheBuilder()
        {
            var id = new Mock<IIdentifier>().Object;
            var item = new Mock<IItem>().Object;
            var factory = new Mock<IFactory>();
            factory.Setup(o => o.CanCreate(id)).Returns(true);
            factory.Setup(o => o.Create(id)).Returns(item);
            IEquationBuilder builder = new EquationBuilder(new[] { factory.Object });

            IItem createItem;
            var returnedBuilder = builder.CreateItem(id, out createItem);

            Assert.AreEqual(builder, returnedBuilder);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateItem_WithFactoriesThatCanCreateSameItem_Fails()
        {
            var id = new Mock<IIdentifier>().Object;
            var item = new Mock<IItem>().Object;
            var factory = new Mock<IFactory>();
            factory.Setup(o => o.CanCreate(id)).Returns(true);
            factory.Setup(o => o.Create(id)).Returns(item);
            IEquationBuilder builder = new EquationBuilder(new[] { factory.Object, factory.Object });

            builder.CreateItem(id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateItemReturningItem_WithFactoriesThatCanCreateSameItem_Fails()
        {
            var id = new Mock<IIdentifier>().Object;
            var item = new Mock<IItem>().Object;
            var factory = new Mock<IFactory>();
            factory.Setup(o => o.CanCreate(id)).Returns(true);
            factory.Setup(o => o.Create(id)).Returns(item);
            IEquationBuilder builder = new EquationBuilder(new[] { factory.Object, factory.Object });

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

        [TestMethod]
        public void Divide_WithExpressionAndFactory_AllowsCreatingNewItemsInsideExpression()
        {
            var id = new Mock<IIdentifier>().Object;
            var item = new Mock<IItem>().Object;
            var factory = new Mock<IFactory>(MockBehavior.Strict);
            factory.Setup(o => o.CanCreate(id)).Returns(true);
            factory.Setup(o => o.Create(id)).Returns(item);
            IEquationBuilder builder = new EquationBuilder(new[] { factory.Object });

            builder = builder.Divide(b => b.CreateItem(id));

            factory.VerifyAll();
        }

        [TestMethod]
        public void ToImmutableTree_WithParenthesesAndValue_ReturnsTreeWithParenthesesAndValue()
        {
            IEquationBuilder builder = new EquationBuilder();
            builder = builder.Parentheses(b => b.Value(3));

            var tree = (builder as EquationBuilder).ToImmutableTree();

            var parenthesis = tree.GetChildren(tree.GetRoot()).First();
            var value = tree.GetChildren(parenthesis).First();
            Assert.AreEqual("()", tree.ValueOf(parenthesis));
            Assert.AreEqual("3", tree.ValueOf(value));
        }

        [TestMethod]
        public void Parentheses_WithExpressionAndFactory_AllowsCreatingNewItemsInsideExpression()
        {
            var id = new Mock<IIdentifier>().Object;
            var item = new Mock<IItem>().Object;
            var factory = new Mock<IFactory>(MockBehavior.Strict);
            factory.Setup(o => o.CanCreate(id)).Returns(true);
            factory.Setup(o => o.Create(id)).Returns(item);
            IEquationBuilder builder = new EquationBuilder(new[] { factory.Object });

            builder = builder.Parentheses(b => b.CreateItem(id));

            factory.VerifyAll();
        }
    }
}
