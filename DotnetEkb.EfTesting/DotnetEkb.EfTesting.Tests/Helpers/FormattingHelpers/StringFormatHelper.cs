namespace DotnetEkb.EfTesting.Tests.Helpers.FormattingHelpers
{
    public static class StringFormatHelper
    {
        public static string NotNegativeInterval(int? min, int? max)
        {
            if (min == max && min != null)
            {
                return $"Ровно {min}";
            }
            if (min == 0 || min == null)
            {
                return max == null ? "Любое" : $"Не более {max}";
            }
            return max == null ? $"Не менее {min}" : $"От {min} до {max}";
        }

        public static string GetDashIfNullValue<T>(this T value)
        {
            return value != null ? value.ToString() : "-";
        }
    }
}