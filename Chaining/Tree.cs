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

        public Tree()
            : this (new KeyProvider())
        {
        }

        public Tree(IKeyProvider keyProvider)
        {
            if (keyProvider == null)
            {
                throw new ArgumentException("keyProvider can't be null");
            }

            KeyProvider = keyProvider;
            state = BuildInitialState();
        }

        private Tree(IKeyProvider keyProvider, State<T> state)
        {
            this.KeyProvider = keyProvider;
            this.state = state;
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

        public Tree<T> AddNode(T value, KeyType parentKey)
        {
            KeyType key;
            return AddNode(value, parentKey, out key);
        }

        public Tree<T> AddNode(T value, KeyType parentKey, out KeyType key)
        {
            KeyType actualKey;
            if (!state.keyToValueDictionary.TryGetKey(parentKey, out actualKey))
            {
                throw new ArgumentException("parentKey not found");
            }

            State<T> newState = state;

            key = KeyProvider.Key();
            newState.keyToValueDictionary = newState.keyToValueDictionary.Add(key, value);

            newState.childToParentDictionary = newState.childToParentDictionary.SetItem(key, parentKey);

            ImmutableArray<KeyType> children;
            if (!newState.parentToChildrenDictionary.TryGetValue(parentKey, out children))
            {
                children = ImmutableArray<KeyType>.Empty;
            }
            children = children.Add(key);
            newState.parentToChildrenDictionary = newState.parentToChildrenDictionary.SetItem(parentKey, children);

            return new Tree<T>(KeyProvider, newState);
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

        public Tree<T> AddRoot(T value)
        {
            KeyType key;
            return AddRoot(value, out key);
        }

        public Tree<T> AddRoot(T value, out KeyType key)
        {
            State<T> newState = state;

            key = KeyProvider.Key();
            newState.rootKey = key;
            newState.keyToValueDictionary = state.keyToValueDictionary.Add(key, value);

            return new Tree<T>(KeyProvider, newState);
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
