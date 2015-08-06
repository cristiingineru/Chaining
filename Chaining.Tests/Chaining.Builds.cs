using System;

namespace Chaining.Tests
{
    public class Chaining
    {
		private void SnippentsThatDontBreakTheBuild()
		{
			Action<string> destination = number => Console.Write(number);

            {
                var builder = new EquationBuilder(destination);

                builder
                    .Value(1)
                    .Add()
                    .Value(2);
            }

            {
                var builder = new EquationBuilder(destination);

                builder
                    .Literal("x")
                    .Add()
                    .Literal("y");
            }

            {
                var builder = new MarkupEquationBuilder(destination);

                builder
                    .Value(1)
                    .Add()
                    .Bold(b => b.Value(2))
                    .Add()
                    .Value(3);
            }

            {
                var builder = new MarkupEquationBuilder(destination);

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
                var builder = new MarkupEquationBuilder(destination);

                builder
                    .Value(1)
                    .Add()
                    .Bold(b => b.Literal("x"))
                    .Add()
                    .Value(3);
            }

            {
                var builder = new EquationBuilder(destination);

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
                var builder = new EquationBuilder(destination);

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
                var builder = new EquationBuilder(destination);

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
                var builder = new EquationBuilder(destination);
                var adding = true;

                builder
                    .Value(1)
                    .If(adding,
                        True: b => b.Add(),
                        False: b => b.Divide())
                    .Value(2);
            }

            {
                var builder = new EquationBuilder(destination);
                var needExtraValues = true;

                builder
                    .Value(1)
					.Add()
                    .If(needExtraValues,
                        True: b => b.Value(3).Add().Value(4).Add())
                    .Value(2);
            }
        }
    }
}
