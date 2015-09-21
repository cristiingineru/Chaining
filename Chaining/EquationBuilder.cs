using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyType = System.Int32;

namespace Chaining
{
    public class EquationBuilder : IEquationBuilder
    {
        private IEnumerable<IFactory> Factories { get; set; }
        private Tree<String> Tree { get; set; }
        private KeyType Root { get; set; }

        public EquationBuilder()
            : this (Enumerable.Empty<IFactory>())
        {

        }

        public EquationBuilder(IEnumerable<IFactory> factories)
            : this (factories, null)
        {
        }

        private EquationBuilder(IEnumerable<IFactory> factories, string rootValue)
        {
            Factories = factories;
            Tree = BuildDefaultTree(rootValue);
            Root = Tree.GetRoot();
        }

        private Tree<string> BuildDefaultTree(string rootValue)
        {
            var tree = new Tree<string>();
            rootValue = rootValue ?? "[equation builder]";   

            return tree.AddRoot(rootValue);
        }

        public EquationBuilder CreateItem(IIdentifier identifier)
        {
            IItem item;
            return CreateItem(identifier, out item);
        }
        public EquationBuilder CreateItem(IIdentifier identifier, out IItem item)
        {
            var selectedFactories = Factories.Where(factory => factory.CanCreate(identifier));
            
            switch(selectedFactories.Count())
            {
                case 0:
                    throw new ArgumentException("no factory available");
                case 1: item = selectedFactories.First().Create(identifier);
                    break;
                default:
                    throw new ArgumentException("more than one factory can create a such item");
            }

            return this;
        }

        public Tree<string> ToImmutableTree()
        {
            return Tree;
        }

        public EquationBuilder Value(int constant)
        {
            Tree = Tree.AddNode(constant.ToString(), Root);
            return this;
        }
        public EquationBuilder Literal(string variable)
        {
            Tree = Tree.AddNode(variable, Root);
            return this;
        }
        public EquationBuilder Add()
        {
            Tree = Tree.AddNode("+", Root);
            return this;
        }
        public EquationBuilder Divide()
        {
            Tree = Tree.AddNode("/", Root);
            return this;
        }
        public EquationBuilder Divide(Action<EquationBuilder> expression)
        {
            return ScriptNestedExpression(expression, "/");
        }
        public EquationBuilder Parentheses(Action<EquationBuilder> expression)
        {
            return ScriptNestedExpression(expression, "()");
        }

        private EquationBuilder ScriptNestedExpression(Action<EquationBuilder> expression, string rootValue)
        {
            var b = new EquationBuilder(this.Factories, rootValue);

            expression(b);

            var sourceTree = b.Tree;
            var sourceRoot = b.Tree.GetRoot();
            Tree = Tree.CopyBranch(sourceTree, sourceRoot, Root);

            return this;
        }

    }
}
