using System;
using System.Collections.Generic;

namespace Chaining
{

    public interface IEquationBuilder { }

    public class EquationBuilder : IEquationBuilder
    {
        public EquationBuilder(Action<string> writer)
        {

        }
    }
    
    public class MarkupEquationBuilder : IEquationBuilder
    {
        public MarkupEquationBuilder(Action<string> writer)
        {

        }
    }


    public interface IValue
    {
        int GetData();
    }

    public class Value : IValue
    {
        private int data;

        public int GetData()
        {
            return data;
        }

        public Value SetData(int data)
        {
            this.data = data;
            return this;
        }
    }


    public static class Operations
    {
        public static T Value<T>(this T builder, int constant) where T : IEquationBuilder
        {
            return builder;
        }
        public static T Value<T>(this T builder, int constant, out IValue value) where T : IEquationBuilder
        {
            value = (new Value()).SetData(constant);
            return builder;
        }

        public static T Literal<T>(this T builder, string variable) where T : IEquationBuilder
        {
            return builder;
        }
        public static T Add<T>(this T builder) where T : IEquationBuilder
        {
            return builder;
        }
        public static T Divide<T>(this T builder) where T : IEquationBuilder
        {
            return builder;
        }
        public static T Divide<T>(this T builder, Action<T> expression) where T : IEquationBuilder
        {
            return builder;
        }

        public static MarkupEquationBuilder Bold(this MarkupEquationBuilder builder, Action<MarkupEquationBuilder> expression)
        {
            return builder;
        }

        public static T Parentheses<T>(this T builder, Action<T> expression) where T : IEquationBuilder
        {
            return builder;
        }

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
