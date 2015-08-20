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
        private IKeyProvider keyProvider;
        private KeyType rootKey;
        private ImmutableDictionary<KeyType, T> keyToValueDictionary;
        private ImmutableDictionary<KeyType, KeyType> childToParentDictionary;
        private ImmutableDictionary<KeyType, ImmutableArray<KeyType>> parentToChildrenDictionary;

        public Tree(IKeyProvider keyProvider)
        {
            if (keyProvider == null)
            {
                throw new ArgumentException("keyProvider can't be null");
            }

            this.keyProvider = keyProvider;

            rootKey = InvalidKey();
            keyToValueDictionary = ImmutableDictionary<KeyType, T>.Empty;
            childToParentDictionary = ImmutableDictionary<KeyType, KeyType>.Empty;
            parentToChildrenDictionary = ImmutableDictionary<KeyType, ImmutableArray<KeyType>>.Empty;
        }

        public KeyType AddNode(T value)
        {
            var key = keyProvider.Key();
            keyToValueDictionary = keyToValueDictionary.Add(key, value);
            return key;
        }

        public T ValueOf(KeyType key)
        {
            T value;
            if (!keyToValueDictionary.TryGetValue(key, out value))
            {
                throw new ArgumentException("key not found");
            }
            return value;
        }

        public void SetParent(KeyType childKey, KeyType parentKey)
        {
            KeyType actualKey;
            if (!keyToValueDictionary.TryGetKey(childKey, out actualKey))
            {
                throw new ArgumentException("childKey not found");
            }
            if (!keyToValueDictionary.TryGetKey(parentKey, out actualKey))
            {
                throw new ArgumentException("parentKey not found");
            }

            childToParentDictionary = childToParentDictionary.SetItem(childKey, parentKey);

            ImmutableArray<KeyType> children;
            if (!parentToChildrenDictionary.TryGetValue(parentKey, out children))
            {
                children = ImmutableArray<KeyType>.Empty;
            }
            children = children.Add(childKey);
            parentToChildrenDictionary = parentToChildrenDictionary.SetItem(parentKey, children);
        }

        public KeyType GetParent(KeyType childKey)
        {
            KeyType parentKey;
            if (!childToParentDictionary.TryGetValue(childKey, out parentKey))
            {
                throw new ArgumentException("childKey not found");
            }
            return parentKey;
        }

        public KeyType AddRoot(T value)
        {
            rootKey = AddNode(value);
            return rootKey;
        }

        public KeyType GetRoot()
        {
            return rootKey;
        }

        public IEnumerable<KeyType> GetChildren(KeyType parentKey)
        {
            ImmutableArray<KeyType> children;
            if (parentToChildrenDictionary.TryGetValue(parentKey, out children))
            {
                return children.AsEnumerable();
            }
            return Enumerable.Empty<KeyType>();
        }


        private KeyType InvalidKey()
        {
            return -1;
        }
    }
}
