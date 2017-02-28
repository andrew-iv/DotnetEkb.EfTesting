using System.Collections.Generic;
using System.Linq;

namespace DotnetEkb.EfTesting.Tests.Helpers.ComparerHelpers
{
    /// <summary>
    /// Сравнивает, используя несколько компараторов.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public class CombinedComparer<TElement> : IComparer<TElement>
    {
        public CombinedComparer(IEnumerable<IComparer<TElement>> comparers)
        {
            _comparers = comparers.ToList();
        }

        private readonly List<IComparer<TElement>> _comparers;

        public int Compare(TElement x, TElement y)
        {
            return _comparers.Select(comparer => comparer.Compare(x, y)).FirstOrDefault(compRes => compRes != 0);
        }
    }
}