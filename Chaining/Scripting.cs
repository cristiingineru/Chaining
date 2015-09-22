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
        bool CanCreate(IIdentifier identifier);
        IItem Create(IIdentifier identifier);
    }


    /// <summary>
    /// This is the static contract for each EquationBuilder implementation. We are relying on the runtime contract only
    /// to allow channing without losing the initial type.
    /// 
    /// Any method specified here has to be stronly typed thus all the implementers would be forced to returned the same type.
    /// </summary>
    public interface IEquationBuilder
    {

    }

    /// <summary>
    /// This is a dynamic contract. Each class that implements the IEquationBuilder has to implement all
    /// the methods specified here otherwise a client would get a run time exception when using a such not implemented method.
    /// 
    /// The functions are generic for returning the exact passed type so channing multiple operations will not
    /// lose the initial type.
    /// </summary>
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
}
