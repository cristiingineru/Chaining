using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chaining.Tests
{
    [TestClass]
    public class EquationBuilderTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EquationBuilderWithNoFactory_CreateItem_Fails()
        {
            IEquationBuilder builder = new EquationBuilder();

            builder.CreateItem(new TestIdentifier());
        }

        private class TestIdentifier : IIdentifier
        {

        }
    }
}
