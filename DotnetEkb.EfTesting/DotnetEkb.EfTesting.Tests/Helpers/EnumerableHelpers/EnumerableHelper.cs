using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotnetEkb.EfTesting.Tests.Helpers.ExpressionHelpers;

namespace DotnetEkb.EfTesting.Tests.Helpers.EnumerableHelpers
{
    public static class EnumerableHelper
    {
        public static IEnumerable<T> WhereNotContains<T>(this IEnumerable<T> value, IEnumerable<T> valList)
        {
            return value.Where(val => !valList.Contains(val));
        }

        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> value)
        {
            return value.Where(val => val != null);
        }

        public static TRes MaxOrDefault<T, TRes>(this IEnumerable<T> value, Func<T, TRes> selector)
        {
            return value.IsEmpty() ? default(TRes) : value.Max(selector);
        }

        public static T[] NullIfEmtpyOrNulls<T>(this IEnumerable<T> value) where T : class
        {
            if (value == null)
                return null;
            var res = value.NotNull().ToArray();
            return res.Any() ? res : null;
        }

        public static IQueryable<T> NotNull<T>(this IQueryable<T> value) where T : class
        {
            return value.Where(val => val != null).Fix();
        }


        public static T GetValue<T, TKey>(this IDictionary<TKey, T> dictionary, TKey key, T @default = default(T))
        {
            T value;
            return dictionary.TryGetValue(key, out value) ? value : @default;
        }

        public static T GetValueOrCreate<T, TKey>(this IDictionary<TKey, T> dictionary, TKey key, Func<T> factory)
        {
            T value;
            return dictionary.TryGetValue(key, out value) ? value : (dictionary[key] = factory());
        }

        public static T GetValue<T, TKey>(this IDictionary<TKey, T> dictionary, TKey key, Func<T> factory)
        {
            T value;
            return dictionary.TryGetValue(key, out value) ? value : factory();
        }

        public static IEnumerable<T> Each<T>(this IEnumerable<T> collection, Action<T> func)
        {
            var enumerable = collection as T[] ?? collection.ToArray();
            foreach (var item in enumerable)
            {
                func(item);
            }
            return enumerable;
        }

        public static bool IsPeriodsIntersected<T, TType>(this IEnumerable<T> collection, Func<T, TType> start, Func<T, TType> end, IComparer<TType> comparer = null)
        {
            comparer = comparer ?? Comparer<TType>.Default;
            return IsAnyForStartAndPrevEnd(collection, start, end, (st, lastEnd) => (lastEnd == null || st == null || comparer.Compare(st, lastEnd) <= 0));
        }

        public static bool IsAnyForStartAndPrevEnd<T, TType>(this IEnumerable<T> collection, Func<T, TType> start, Func<T, TType> end, Func<TType, TType, bool> condition, IComparer<TType> comparer = null)
        {
            comparer = comparer ?? Comparer<TType>.Default;
            var arr = collection.Select(x => new
            {
                st = start(x),
                end = end(x)
            }).OrderBy(x => x.st, comparer).ToArray();

            for (int i = 1; i < arr.Length; i++)
            {
                var lastEnd = arr[i - 1].end;
                var st = arr[i].st;
                if (condition(st, lastEnd))
                    return true;
            }
            return false;
        }

        public static bool AllEquals<T>(this IEnumerable<T> enumerable)
        {
            var enumerator = enumerable.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return true;
            }
            var first = enumerator.Current;
            while (enumerator.MoveNext())
            {
                if (!first.Equals(enumerator.Current))
                {
                    return false;
                }
            }
            return true;
        }

        public static IEnumerable<T> GetItemsAt<T>(this IList<T> list, IEnumerable<int> indexes)
        {
            return indexes.Select(index => list[index]);
        }

        public static IEnumerable<T> GetItemsAt<T, TKey>(this IDictionary<TKey, T> dict, IEnumerable<TKey> keys, bool ignoreNotExistingKeys = false)
        {
            return keys.Select(key =>
            {
                T val;
                dict.TryGetValue(key, out val);
                return val;
            });
        }

        /// <summary>
        /// Выдает пару предыдущий/последующий элемент
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="selector">на вход пара (a[i-1],a[i]) </param>
        /// <returns></returns>
        public static IEnumerable<TResult> SelectAsPairs<T, TResult>(this IEnumerable<T> enumerable, Func<T, T, TResult> selector)
        {
            var enumerator = enumerable.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                yield break;
            }
            var next = enumerator.Current;
            if (!enumerator.MoveNext())
            {
                yield break;
            }
            while (enumerator.MoveNext())
            {
                var prev = next;
                next = enumerator.Current;
                yield return selector(prev, next);
            }
        }

        /// <summary>
        /// Выдает пару предыдущий/последующий элемент
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<T, T>> SelectAsPairs<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.SelectAsPairs((a, b) => new Tuple<T, T>(a, b));
        }

        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        /// <summary>
        /// Все ли элементы уникальны?
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool IsUnique<T>(this IEnumerable<T> items)
        {
            var hs = new HashSet<T>();
            foreach (var item in items)
            {
                if (hs.Contains(item))
                {
                    return false;
                }
                hs.Add(item);
            }
            return true;
        }

        /// <summary>
        /// Повторяет N раз
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"> </param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static IEnumerable<T> Duplicate<T>(this IEnumerable<T> @this, int n)
        {
            return Enumerable.Range(1, n).SelectMany(x => @this);
        }

        /// <summary>
        /// Проверяет на равенство множеств
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool SetEqualsComparable<T>(this IEnumerable<T> @this, IEnumerable<T> other) where T : IComparable
        {
            if (@this == null)
            {
                return other == null;
            }
            return other != null && @this.Distinct().OrderBy(x => x).SequenceEqual(other.Distinct().OrderBy(x => x));
        }

        public static bool IsOrderedBy<T, TProperty>(this IEnumerable<T> list, Func<T, TProperty> keySelector) where TProperty : IComparable<TProperty>
        {
            IComparable<TProperty> previousValue = null;
            foreach (var item in list)
            {
                var currentValue = keySelector.Invoke(item);
                if (previousValue == null)
                {
                    previousValue = currentValue;
                    continue;
                }

                if (previousValue.CompareTo(currentValue) > 0)
                {
                    return false;
                }
                previousValue = currentValue;
            }
            return true;
        }

        public static string Join<T>(this IEnumerable<T> enumerable, string separator)
        {
            var sb = new StringBuilder();
            enumerable.Each(element => sb.AppendFormat("{0}{1}", element, separator));
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - separator.Length, separator.Length);
            }
            return sb.ToString();
        }

        public static IEnumerable<T> DistinctBy<T, TBy>(this IEnumerable<T> enumerable, Func<T, TBy> by)
        {
            return enumerable.GroupBy(by).Select(x => x.First());
        }

        public static IEnumerable<T> AddFirst<T>(this IEnumerable<T> enumerable, T element)
        {
            return new[] { element }.Concat(enumerable);
        }
    }
}