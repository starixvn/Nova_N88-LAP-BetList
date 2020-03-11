namespace Fanex.Athena.Common.Utility.Extensions
{
    using System.Collections.Generic;

    public static class DictionaryExtension
    {
        public static TValue GetValueOrDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue defaultValue)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }
    }
}