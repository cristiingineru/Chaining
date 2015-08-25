using System;
using System.Collections.Generic;

namespace Chaining
{

    public interface IItem
    {

    }

    public interface IIdentifier
    {

    }

    public interface IFactory
    {
        IItem Create(IIdentifier identifier);
    }


    public interface IEquationBuilder
    {
        IEquationBuilder RegisterFactory(IFactory factory);
    }

    public static class IEquationBuilderOperations
    {
        public static T Value<T>(this T builder, int constant) where T : IEquationBuilder
        {
            return (builder as dynamic).Value(constant);
        }
        public static T Literal<T>(this T builder, string variable) where T : IEquationBuilder
        {
            return (builder as dynamic).Literal(variable);
        }
        public static T Add<T>(this T builder) where T : IEquationBuilder
        {
            return (builder as dynamic).Add();
        }
        public static T Divide<T>(this T builder) where T : IEquationBuilder
        {
            return (builder as dynamic).Divide();
        }
        public static T Divide<T>(this T builder, Action<T> expression) where T : IEquationBuilder
        {
            return (builder as dynamic).Divide(expression);
        }
        public static T Parentheses<T>(this T builder, Action<T> expression) where T : IEquationBuilder
        {
            return (builder as dynamic).Parentheses(expression);
        }

        public static T CreateItem<T>(this T builder, IIdentifier identifier) where T : IEquationBuilder
        {
            return (builder as dynamic).CreateItem(identifier);
        }
        public static T CreateItem<T>(this T builder, IIdentifier identifier, out IItem item) where T : IEquationBuilder
        {
            return (builder as dynamic).CreateItem(identifier, out item);
        }
        public static T CreateItem<U, T>(this T builder, IIdentifier identifier, out U item)
            where U : IItem
            where T : IEquationBuilder
        {
            return (builder as dynamic).CreateItem(identifier, out item);
        }
    }


    public class EquationBuilder : IEquationBuilder
    {
        public EquationBuilder()
        {

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

    
    public class MarkupEquationBuilder : IEquationBuilder
    {
        public MarkupEquationBuilder()
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


    public static class ProgrammingOperations
    {
        public static T If<T>(this T builder, bool condition, Action<T> onTrue, Action<T> onFalse = null) where T : IEquationBuilder
        {
            return builder;
        }

        public static T Map<T, U>(this T builder, IEnumerable<U> collection, Action<T, U> action) where T : IEquationBuilder
        {
            return builder;
        }

        public static T Map<T, U>(this T builder, IEnumerable<U> collection, Action<T, U, int> action) where T : IEquationBuilder
        {
            return builder;
        }

        public static T Reduce<T, U, V>(this T builder, IEnumerable<U> collection, V seed, Func<U, V, V> action, Action<T, V> onEnd) where T : IEquationBuilder
        {
            return builder;
        }

        public static T Reduce<T, U, V>(this T builder, IEnumerable<U> collection, V seed, Func<U, V, V, int> action, Action<T, V> onEnd) where T : IEquationBuilder
        {
            return builder;
        }
    }
}
