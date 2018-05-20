using System.Collections.Generic;

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
        public bool Remove(TKey key, TValue value)
        {
            if (ContainsKey(key))
            {             
              return this[key].Remove(value);
            }

            return false;
        }
    }
}
