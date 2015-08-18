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
        private ImmutableDictionary<KeyType, T> data;

        public Tree(IKeyProvider keyProvider)
        {
            if (keyProvider == null)
            {
                throw new ArgumentException("keyProvider can't be null");
            }

            this.keyProvider = keyProvider;

            this.data = ImmutableDictionary<KeyType, T>.Empty;
        }

        public KeyType Add(T value)
        {
            var key = keyProvider.Key();
            data = data.Add(key, value);
            return key;
        }

        public T Get(KeyType key)
        {
            T value;
            if (!data.TryGetValue(key, out value))
            {
                throw new ArgumentException("key not found");
            }
            return value;
        }
    }
}
