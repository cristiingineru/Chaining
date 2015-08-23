using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyType = System.Int32;

namespace Chaining
{
    public class Tree<T>
    {
        public IKeyProvider KeyProvider { get; private set; }

        private State<T> state;

        private struct State<U>
        {
            public KeyType rootKey;
            public ImmutableDictionary<KeyType, U> keyToValueDictionary;
            public ImmutableDictionary<KeyType, KeyType> childToParentDictionary;
            public ImmutableDictionary<KeyType, ImmutableArray<KeyType>> parentToChildrenDictionary;
        }

        public Tree(IKeyProvider keyProvider)
        {
            if (keyProvider == null)
            {
                throw new ArgumentException("keyProvider can't be null");
            }

            this.KeyProvider = keyProvider;

            state = BuildInitialState();
        }

        private State<T> BuildInitialState()
        {
            var initialState = new State<T>();

            initialState.rootKey = KeyProvider.InvalidKey();
            initialState.keyToValueDictionary = ImmutableDictionary<KeyType, T>.Empty;
            initialState.childToParentDictionary = ImmutableDictionary<KeyType, KeyType>.Empty;
            initialState.parentToChildrenDictionary = ImmutableDictionary<KeyType, ImmutableArray<KeyType>>.Empty;

            return initialState;
        }

        public T ValueOf(KeyType key)
        {
            T value;
            if (!state.keyToValueDictionary.TryGetValue(key, out value))
            {
                throw new ArgumentException("key not found");
            }
            return value;
        }

        public KeyType AddNode(T value, KeyType parentKey)
        {
            KeyType actualKey;
            if (!state.keyToValueDictionary.TryGetKey(parentKey, out actualKey))
            {
                throw new ArgumentException("parentKey not found");
            }

            var nodeKey = KeyProvider.Key();
            state.keyToValueDictionary = state.keyToValueDictionary.Add(nodeKey, value);

            state.childToParentDictionary = state.childToParentDictionary.SetItem(nodeKey, parentKey);

            ImmutableArray<KeyType> children;
            if (!state.parentToChildrenDictionary.TryGetValue(parentKey, out children))
            {
                children = ImmutableArray<KeyType>.Empty;
            }
            children = children.Add(nodeKey);
            state.parentToChildrenDictionary = state.parentToChildrenDictionary.SetItem(parentKey, children);

            return nodeKey;
        }

        public KeyType GetParent(KeyType childKey)
        {
            KeyType parentKey;
            if (!state.childToParentDictionary.TryGetValue(childKey, out parentKey))
            {
                throw new ArgumentException("childKey not found");
            }
            return parentKey;
        }

        public KeyType AddRoot(T value)
        {
            state.rootKey = KeyProvider.Key();
            state.keyToValueDictionary = state.keyToValueDictionary.Add(state.rootKey, value);
            return state.rootKey;
        }

        public KeyType GetRoot()
        {
            return state.rootKey;
        }

        public IEnumerable<KeyType> GetChildren(KeyType parentKey)
        {
            ImmutableArray<KeyType> children;
            if (state.parentToChildrenDictionary.TryGetValue(parentKey, out children))
            {
                return children.AsEnumerable();
            }
            return Enumerable.Empty<KeyType>();
        }
    }
}
