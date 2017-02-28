using System;

namespace DotnetEkb.EfTesting.CommonData.Models
{
    public class Interval<TType> where TType : struct, IComparable
    {
        public TType? From { get; set; }

        public TType? To { get; set; }

        public bool IsEmpty => From != null && To != null && From.Value.CompareTo(To) > 0;
    }
}
