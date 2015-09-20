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
        private KeyType CurrentNodeKey { get; set; }

        public EquationBuilder()
            : this (Enumerable.Empty<IFactory>())
        {

        }

        public EquationBuilder(IEnumerable<IFactory> factories)
        {
            Factories = factories;
            Tree = BuildDefaultTree();
            CurrentNodeKey = Tree.GetRoot();
        }

        private Tree<string> BuildDefaultTree()
        {
            var tree = new Tree<string>();

            return tree.AddRoot("[equation builder]");
        }

        private EquationBuilder(Func<Tree<String>> defaultTreeBuilder)
        {
            Factories = Enumerable.Empty<IFactory>();
            Tree = defaultTreeBuilder();
            CurrentNodeKey = Tree.GetRoot();
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
            Tree = Tree.AddNode(constant.ToString(), CurrentNodeKey);
            return this;
        }
        public EquationBuilder Literal(string variable)
        {
            Tree = Tree.AddNode(variable, CurrentNodeKey);
            return this;
        }
        public EquationBuilder Add()
        {
            Tree = Tree.AddNode("+", CurrentNodeKey);
            return this;
        }
        public EquationBuilder Divide()
        {
            Tree = Tree.AddNode("/", CurrentNodeKey);
            return this;
        }
        public EquationBuilder Divide(Action<EquationBuilder> expression)
        {
            var keyProvider = this.Tree.KeyProvider;
            var b = new EquationBuilder(() =>
            {
                return new Tree<string>(keyProvider)
                    .AddRoot("/");
            });

            expression(b);

            var sourceTree = b.Tree;
            var source = sourceTree.GetRoot();
            Tree = Tree.CopyBranch(sourceTree, source, CurrentNodeKey);

            return this;
        }
        public EquationBuilder Parentheses(Action<EquationBuilder> expression)
        {
            return this;
        }
    }
}
