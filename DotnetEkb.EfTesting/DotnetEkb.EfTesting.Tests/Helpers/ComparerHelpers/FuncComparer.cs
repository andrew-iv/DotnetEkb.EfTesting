using System;
using System.Collections.Generic;

namespace DotnetEkb.EfTesting.Tests.Helpers.ComparerHelpers
{
    /// <summary>
    /// Сравнивает результаты функции.  
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class FuncComparer<TElement, TKey> : IComparer<TElement>
    {
        public FuncComparer(Func<TElement, TKey> keySelector, IComparer<TKey> comparer = null)
        {
            this._keySelector = keySelector;
            this._comparer = comparer ?? Comparer<TKey>.Default;
        }

        private readonly IComparer<TKey> _comparer;
        private readonly Func<TElement, TKey> _keySelector;

        public int Compare(TElement x, TElement y)
        {
            var keyX = _keySelector(x);
            var keyY = _keySelector(y);
            return _comparer.Compare(keyX, keyY);
        }
    }
}