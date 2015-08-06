# Chaining
Playground for testing function chaining API for equation builders. The main focus of this project is API experimentation so the builders are not producing anything but they could.


##Examples:

```cs
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
		.Bold(localBuilder => localBuilder.Value(2))
		.Add()
		.Value(3);
}

{
	var builder = new MarkupEquationBuilder(destination);

	builder
		.Value(1)
		.Add()
		.Bold(localBuilder =>
			{
				localBuilder
					.Value(2)
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
		.Bold(localBuilder => localBuilder.Literal("x"))
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
		.Parentheses(localBuilder =>
		{
			localBuilder
			   .Value(2)
			   .Add()
			   .Value(2);
		});
}

{
	var builder = new EquationBuilder(destination);

	builder
		.Value(1)
		.Divide()
		.Parentheses(localBuilder =>
		{
			localBuilder
			   .Value(2)
			   .Add()
			   .Value(3);
		});
}

{
	var builder = new EquationBuilder(destination);

	builder
		.Value(1)
		.Divide(localBuilder =>
		{
			localBuilder
				.Value(2)
				.Add()
				.Value(3);
		});
}
```
