using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email_client.Model
{
    public class ImprovedDictionary<TKey, TValue> : Dictionary<TKey, List<TValue>>
    {
        public void Add(TKey key, TValue value)
        {
            if (ContainsKey(key))
            {
                if(!this[key].Contains(value))
                this[key].Add(value);
            }
            else             
                Add(key, new List<TValue> { value });
        }
    }
}
