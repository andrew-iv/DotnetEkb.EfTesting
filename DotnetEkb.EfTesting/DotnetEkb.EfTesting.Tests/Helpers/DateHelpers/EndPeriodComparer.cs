using System;
using System.Collections.Generic;

namespace DotnetEkb.EfTesting.Tests.Helpers.DateHelpers
{
    /// <summary>
    /// Сравнивает даты. Интерпретирую null как бесконечность
    /// </summary>
    public class EndDateComparer : IComparer<DateTime?>
    {
        public int Compare(DateTime? x, DateTime? y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null)
                return 1;
            if (y == null)
                return -1;
            return DateTime.Compare(x.Value, y.Value);
        }
    }
}