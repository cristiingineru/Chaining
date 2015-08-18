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
    }

    public class KeyProvider : IKeyProvider
    {
        private KeyType counter;

        public KeyType Key()
        {
            var key = counter;
            counter += 1;
            return key;
        }
    }
}
