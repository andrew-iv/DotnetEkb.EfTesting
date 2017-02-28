using System.Collections.Generic;
using DotnetEkb.EfTesting.Tests.Helpers.EnumerableHelpers;

namespace DotnetEkb.EfTesting.Tests.Helpers.CollectionHelpers
{
    public static class CollectionHelper
    {
        public static void AddAll<TElement>(this ICollection<TElement> self, IEnumerable<TElement> other)
        {
            other.Each(self.Add);
        }

        public static void AddAll<TKey, TValue>(this IDictionary<TKey, TValue> self, IEnumerable<KeyValuePair<TKey, TValue>> other)
        {
            other.Each(self.Add);
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : default(TValue);
        }
    }
}