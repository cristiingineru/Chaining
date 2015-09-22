using Microsoft.CSharp.RuntimeBinder;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chaining.Tests
{
    public static class IEquationBuilderDynamicContractChecker
    {
        public static bool IsConform(IEquationBuilder subject)
        {
            var callingTests = new Action<IEquationBuilder>[] {
                s => s.Value(0),
                s => s.Literal("x"),
                s => s.Add(),
                s => s.Divide(),
                s => s.Divide(b => { }),
                s => s.Parentheses(b => { })
            };

            Func<Action<IEquationBuilder>, bool> callingTestWrapper = test => {
                var passed = true;
                try
                {
                    test(subject);
                }
                catch (RuntimeBinderException)
                {
                    passed = false;
                }
                return passed;
            };

            var searchingTest = new Func<IEquationBuilder, MethodInfo>[]
                {
                    s => s.GetType().GetMethod("CreateItem", new Type[] { typeof(IIdentifier) }),
                    s => s.GetType().GetMethod("CreateItem", new Type[] { typeof(IIdentifier), typeof(IItem).MakeByRefType() })
                };

            Func<Func<IEquationBuilder, MethodInfo>, bool> searchingTestWrapper = test => test(subject) != null;

            return callingTests.Select(callingTestWrapper)
                .Concat(searchingTest.Select(searchingTestWrapper))
                .All(result => result == true);
        }
    }
}
