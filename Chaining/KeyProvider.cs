using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaining
{
    public class KeyProvider
    {
        private int counter;

        public int Key()
        {
            var key = counter;
            counter += 1;
            return key;
        }
    }
}
