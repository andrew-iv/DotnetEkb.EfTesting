using System;

namespace DotnetEkb.EfTesting.Tests.Helpers.FormattingHelpers
{
    public static class DecimalExtension
    {
        public static string ToPriceString(this decimal value, bool showZeroesInFractionPart = true)
        {
            if (Math.Abs(value) < 1000)
            {
                return showZeroesInFractionPart ? string.Format( "{0:##0.00}", value ) : string.Format( "{0:##0.##}", value );
            }

            if (Math.Abs(value) < 1000000)
            {
                return showZeroesInFractionPart ? string.Format( "{0:# ##0.00}", value ) : string.Format( "{0:# ##0.##}", value );
            }

            if ( Math.Abs( value ) < 1000000000 )
            {
                return showZeroesInFractionPart ? string.Format( "{0:# ### ##0.00}", value ) : string.Format( "{0:# ### ##0.##}", value );
            }

            return showZeroesInFractionPart ? string.Format( "{0:# ### ### ##0.00}", value ) : string.Format( "{0:# ### ### ##0.##}", value );
        }

        public static string ToShortPriceString(this decimal value)
        {
            var abbreviations = new[] { "", " тыс.", " млн."};
            var absValue = Math.Abs(value);
            for (int i = 0, thousands = 1; i < abbreviations.Length; ++i, thousands *= 1000)
            {
                if (absValue < thousands*1000)
                {
                    return string.Format("{0:0.00}{1}", value/thousands, abbreviations[i]);
                }
            }
            return string.Format("{0:0.00} млрд.", value / (1000 * 1000 * 1000));
        }

        public static decimal ToPriceDecimal(this decimal value)
        {
            return decimal.Round(value, 2, MidpointRounding.AwayFromZero);
        }
    }
}