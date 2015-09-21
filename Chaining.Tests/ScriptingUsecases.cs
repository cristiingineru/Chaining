using System;
using System.Linq;

namespace Chaining.Tests
{
    public class Chaining
    {
		private void SnippentsThatDontBreakTheBuild()
		{

            {
                var builder = new EquationBuilder();

                builder
                    .Value(1)
                    .Add()
                    .Value(2);
            }

            {
                var builder = new EquationBuilder();

                builder
                    .Literal("x")
                    .Add()
                    .Literal("y");
            }

            {
                var builder = new MarkupEquationBuilder();

                builder
                    .Value(1)
                    .Add()
                    .Bold(b => b.Value(2))
                    .Add()
                    .Value(3);
            }

            {
                var builder = new MarkupEquationBuilder();

                builder
                    .Value(1)
                    .Add()
                    .Bold(b =>
                        {
                            b.Value(2)
							.Add()
							.Value(3);
                        })
                    .Add()
                    .Value(4);
            }

            {
                var builder = new MarkupEquationBuilder();

                builder
                    .Value(1)
                    .Add()
                    .Bold(b => b.Literal("x"))
                    .Add()
                    .Value(3);
            }

            {
                var builder = new EquationBuilder();

                builder
                    .Value(1)
                    .Add()
                    .Value(2)
                    .Add()
                    .Parentheses(b =>
                    {
                        b.Value(2)
                        .Add()
                        .Value(2);
                    });
            }

            {
                var builder = new EquationBuilder();

                builder
                    .Value(1)
                    .Divide()
                    .Parentheses(b =>
                    {
                        b.Value(2)
                        .Add()
                        .Value(3);
                    });
            }

            {
                var builder = new EquationBuilder();

                builder
                    .Value(1)
                    .Divide(b =>
                    {
                        b.Value(2)
                        .Add()
                        .Value(3);
                    });
            }

            {
                var builder = new EquationBuilder();
                var adding = true;

                builder
                    .Value(1)
                    .If(adding,
                        onTrue: b => b.Add(),
                        onFalse: b => b.Divide())
                    .Value(2);
            }

            {
                var builder = new EquationBuilder();
                var needExtraValues = true;

                builder
                    .Value(1)
					.Add()
                    .If(needExtraValues,
                        onTrue: b => b.Value(3).Add().Value(4).Add())
                    .Value(2);
            }

            {
                var builder = new EquationBuilder();

                builder
                    .Map(Enumerable.Repeat(1, 10), (b, element) =>
                        b.Value(element).Add())
                    .Value(1);
            }

            {
                var builder = new EquationBuilder();

                builder
                    .Map(Enumerable.Range(100, 9),
                        action: (b, element, index) =>
                            b.Value(element)
                            .Divide()
                            .Value(index)
                            .Add())
                    .Value(110)
                    .Divide()
                    .Value(10);
            }

            {
                var builder = new EquationBuilder();

                builder
                    .Reduce(Enumerable.Range(0, 9),
                        seed: 0,
                        action: (previouse, element) => previouse + element,
                        onEnd: (b, final) => b.Value(final));
            }

            {
                var builder = new EquationBuilder();

                builder
                    .Reduce(Enumerable.Range(0, 9),
                        seed: 0,
                        action: (previouse, element, index) => previouse + element * index,
                        onEnd: (b, final) => b.Value(final));
            }
        }
    }
}
