using System;
using System.Linq;

namespace DotnetEkb.EfTesting.Tests.Helpers.MathHelpers
{
    public static class PercentHelper
    {
        public static decimal Calculate(decimal part, decimal total, int? roundDigitsCount = null)
        {
            if (total <= 0 || part <= 0)
            {
                return 0;
            }

            var percent = (part / total) * 100;
            return roundDigitsCount.HasValue ? Math.Round(percent, roundDigitsCount.Value) : percent;
        }

        public static decimal[] Calculate(decimal[] parts, decimal total, int? roundDigitsCount = null)
        {
            var percents = parts.Select(part => Calculate(part, total, roundDigitsCount)).ToArray();
            percents[percents.Length - 1] = 100 - percents.Take(percents.Length - 1).Sum();
            return percents;
        }

        public static double Calculate(double part, double total, int? roundDigitsCount = null)
        {
            if (total <= 0 || part <= 0)
            {
                return 0;
            }

            var percent = (part / total) * 100;
            return roundDigitsCount.HasValue ? Math.Round(percent, roundDigitsCount.Value) : percent;
        }

        public static double[] Calculate(double[] parts, double total, int? roundDigitsCount = null)
        {
            var percents = parts.Select(part => Calculate(part, total, roundDigitsCount)).ToArray();
            percents[percents.Length - 1] = 100 - percents.Take(percents.Length - 1).Sum();
            return percents;
        }

        public static double Calculate(int part, int total, int? roundDigitsCount = null)
        {
            if (total <= 0 || part <= 0)
            {
                return 0;
            }

            return Calculate(part, (double) total, roundDigitsCount);
        }

        public static double[] Calculate(int[] parts, int total, int? roundDigitsCount = null)
        {
            var percents = parts.Select(part => Calculate(part, total, roundDigitsCount)).ToArray();
            percents[percents.Length - 1] = 100 - percents.Take(percents.Length - 1).Sum();
            return percents;
        }
    }
}