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

        public EquationBuilder(Action<string> writer)
        {

        }
    }

    public static class EquationBuilderOperations
    {
        public static EquationBuilder Value(this EquationBuilder builder, int constant)
        {
            return builder;
        }

        public static EquationBuilder Literal(this EquationBuilder builder, string variable)
        {
            return builder;
        }
        public static EquationBuilder Add(this EquationBuilder builder)
        {
            return builder;
        }
        public static EquationBuilder Divide(this EquationBuilder builder)
        {
            return builder;
        }
        public static EquationBuilder Divide(this EquationBuilder builder, Action<EquationBuilder> expression)
        {
            return builder;
        }

        public static EquationBuilder Parentheses(this EquationBuilder builder, Action<EquationBuilder> expression)
        {
            return builder;
        }
    }
    
    public class MarkupEquationBuilder : IEquationBuilder
    {
        public MarkupEquationBuilder()
        {

        }

        public MarkupEquationBuilder(Action<string> writer)
        {

        }
    }

    public static class MarkupEquationBuilderOperations
    {
        public static MarkupEquationBuilder Value(this MarkupEquationBuilder builder, int constant)
        {
            return builder;
        }

        public static MarkupEquationBuilder Literal(this MarkupEquationBuilder builder, string variable)
        {
            return builder;
        }
        public static MarkupEquationBuilder Add(this MarkupEquationBuilder builder)
        {
            return builder;
        }
        public static MarkupEquationBuilder Divide(this MarkupEquationBuilder builder)
        {
            return builder;
        }
        public static MarkupEquationBuilder Divide(this MarkupEquationBuilder builder, Action<MarkupEquationBuilder> expression)
        {
            return builder;
        }

        public static MarkupEquationBuilder Parentheses(this MarkupEquationBuilder builder, Action<MarkupEquationBuilder> expression)
        {
            return builder;
        }

        public static MarkupEquationBuilder Bold(this MarkupEquationBuilder builder, Action<MarkupEquationBuilder> expression)
        {
            return builder;
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
