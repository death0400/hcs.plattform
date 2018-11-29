using System;
using System.Collections.Generic;
namespace System.Collections.Generic
{
    public static class DictionaryExtensions
    {
        public static void AddIfNotExists<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFactory)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, valueFactory());
            }
        }
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }
        public static TValue GetIfExists<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            else
            {
                return default(TValue);
            }
        }
    }
}
