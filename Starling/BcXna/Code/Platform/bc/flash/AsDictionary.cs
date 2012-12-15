using System;
 
using bc.flash;
using System.Collections.Generic;
 
namespace bc.flash
{
	public class AsDictionary : AsObject
	{
        private Dictionary<Object, Object> dictionary;

        public AsDictionary()
        {
            dictionary = new Dictionary<Object, Object>();
        }

        public Object this[Object key]
        {
            get
            {
                Object val = null;
                dictionary.TryGetValue(key, out val);
                return val;
            }
            set
            {
                remove(key);
                dictionary.Add(key, value);
            }
        }

        public bool containsKey(Object key)
        {
            return dictionary.ContainsKey(key);
        }

        public void remove(Object key)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
            }
        }
	}
}
