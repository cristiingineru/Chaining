using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaining
{
    internal class Identifier : IIdentifier
    {

    }

    public class AdvancedMathOperatorsFactory : IFactory
    {

        public bool CanCreate(IIdentifier identifier)
        {
            throw new NotImplementedException();
        }

        public IItem Create(IIdentifier identifier)
        {
            throw new NotImplementedException();
        }
    }

    public static class AdvancedMathOperators
    {
        public static readonly IIdentifier SquareRoot = new Identifier();
    }

    public class SquareRoot
    {
    }
}
