using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chaining.Tests
{
    [TestClass]
    public class AdvancedMathOperatorsTests
    {
        [TestMethod]
        [Ignore]
        public void Factory_PlugedInTheEquationBuilder_CanBeUsedToCreateSquareRoot()
        {
            var factory = new AdvancedMathOperatorsFactory();
            var builder = new EquationBuilder(new[] { factory });

            builder = builder.CreateItem<EquationBuilder>(AdvancedMathOperators.SquareRoot);

            var tree = builder.ToImmutableTree();
            var rootValue = tree.ValueOf(tree.GetRoot());
            Assert.AreEqual("SquareRoot", rootValue);
        }
    }
}
