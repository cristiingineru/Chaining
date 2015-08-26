using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaining
{
    public class EquationBuilder : IEquationBuilder
    {
        public EquationBuilder()
        {

        }

        public EquationBuilder(IEnumerable<IFactory> factories)
        {

        }

        public EquationBuilder CreateItem(IIdentifier identifier)
        {
            throw new ArgumentException("unknown identifier");
        }

        public Tree<string> ToImmutableTree()
        {
            return new Tree<string>();
        }

        public EquationBuilder Value(int constant)
        {
            return this;
        }
        public EquationBuilder Literal(string variable)
        {
            return this;
        }
        public EquationBuilder Add()
        {
            return this;
        }
        public EquationBuilder Divide()
        {
            return this;
        }
        public EquationBuilder Divide(Action<EquationBuilder> expression)
        {
            return this;
        }
        public EquationBuilder Parentheses(Action<EquationBuilder> expression)
        {
            return this;
        }
    }
}
