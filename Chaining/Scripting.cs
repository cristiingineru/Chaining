using System;
using System.Collections.Generic;

namespace Chaining
{

    public interface IEquationBuilder { }

    public class EquationBuilder : IEquationBuilder
    {
        public EquationBuilder()
        {

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
