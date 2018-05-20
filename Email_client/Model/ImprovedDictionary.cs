using System.Collections.Generic;
using System.Linq;

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
            bool result = false;
            if (ContainsKey(key))
            {
                result = this[key].Remove(value);
                if (this[key].Count == 0)
                    Remove(key);
            }

           
            return result;
        }

        public bool RemoveAll()
        {
            var itemsToRemove = this.ToArray();
            foreach (var item in itemsToRemove)
               Remove(item.Key);                
            return true;
        }

        public ImprovedDictionary<TKey, TValue> GetCopy()
        {
            var answer=new ImprovedDictionary<TKey, TValue>();
            foreach (var key in Keys)
            {

                var values = new List<string>();
                foreach (var value in this[key])
                {
                   answer.Add(key, value);
                }
            }

            return answer;
        }
    }
}
