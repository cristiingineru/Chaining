using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyType = System.Int32;

namespace Chaining
{
    public interface IKeyProvider
    {
        KeyType Key();
        KeyType InvalidKey();
        bool IsValid(KeyType key);
    }

    public class KeyProvider : IKeyProvider
    {
        private const KeyType invalidKey = -1;
        private KeyType counter = 0;

        public KeyType Key()
        {
            var key = counter;
            counter += 1;
            return key;
        }

        public int InvalidKey()
        {
            return invalidKey;
        }

        public bool IsValid(KeyType key)
        {
            return key != invalidKey;
        }
    }
}
