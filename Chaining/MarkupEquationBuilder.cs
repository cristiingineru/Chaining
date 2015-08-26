using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaining
{
    public class MarkupEquationBuilder : IEquationBuilder
    {
        public MarkupEquationBuilder()
        {

        }

        public MarkupEquationBuilder(IEnumerable<IFactory> factories)
        {

        }

        public MarkupEquationBuilder Value(int constant)
        {
            return this;
        }
        public MarkupEquationBuilder Literal(string variable)
        {
            return this;
        }
        public MarkupEquationBuilder Add()
        {
            return this;
        }
        public MarkupEquationBuilder Divide()
        {
            return this;
        }
        public MarkupEquationBuilder Divide(Action<MarkupEquationBuilder> expression)
        {
            return this;
        }
        public MarkupEquationBuilder Parentheses(Action<MarkupEquationBuilder> expression)
        {
            return this;
        }
        public MarkupEquationBuilder Bold(Action<MarkupEquationBuilder> expression)
        {
            return this;
        }
    }
}
