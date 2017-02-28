using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DotnetEkb.EfTesting.Tests.Helpers.ComparerHelpers
{
	public class NaturalComparer : Comparer<string>, IDisposable
	{
		private Dictionary<string, string[]> table;

		public NaturalComparer()
		{
			table = new Dictionary<string, string[]>();
		}

		public void Dispose()
		{
			table.Clear();
			table = null;
		}

		public override int Compare(string x, string y)
		{
			if (x == y)
				return 0;
		    if (string.IsNullOrEmpty(x))
		        return -1;
            if (string.IsNullOrEmpty(y))
		        return 1;
		    x = x.Trim();
		    y = y.Trim();

			string[] x1, y1;
			if (!table.TryGetValue(x, out x1))
			{
				x1 = SplitString(x);
				table.Add(x, x1);
			}
			if (!table.TryGetValue(y, out y1))
			{
				y1 = SplitString(y);
				table.Add(y, y1);
			}

			for (int i = 0; i < x1.Length && i < y1.Length; i++)
			{
				if (x1[i] != y1[i])
					return PartCompare(x1[i], y1[i]);
			}
			if (y1.Length > x1.Length)
				return 1;
		    if (x1.Length > y1.Length)
		        return -1;
		    return 0;
		}

		private static int PartCompare(string left, string right)
		{
			int x, y;
			if (!int.TryParse(left, out x) || !int.TryParse(right, out y))
				return string.Compare(left, right, StringComparison.Ordinal);

			return x.CompareTo(y);
		}

		private string[] SplitString(string str)
		{
			return Regex.Split(str.Replace(@"\s+", ""), "([0-9]+)");
		}
	} 
}
