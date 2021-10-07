using System.Collections.Generic;

namespace OpenApi.Smoosh.Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue Rekey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey oldKey, TKey newKey)
        {
            var item = dictionary[oldKey];
            dictionary.Remove(oldKey);
            dictionary.Add(newKey, item);

            return item;
        }
    }
}
