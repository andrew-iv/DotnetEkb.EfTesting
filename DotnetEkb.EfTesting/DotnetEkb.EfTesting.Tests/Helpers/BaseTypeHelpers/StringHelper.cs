using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DotnetEkb.EfTesting.Tests.Helpers.BaseTypeHelpers
{
    public static class StringHelper
    {
        public static bool NullAsEmptyEquals(this string left, string right)
        {
            return (left ?? "") == (right ?? "");
        }

        public static string ToNullIfEmpty(this string str)
        {
            return string.IsNullOrWhiteSpace(str) ? null : str;
        }

        public static string StripHtml(this string inputString)
        {
            return Regex.Replace(inputString, HtmlTagPattern, string.Empty);
        }

        public static string CutTo(this string @this, int len, string postfix = "")
        {
            if (@this.Length <= len)
                return @this;
            return @this.Substring(0, Math.Min(len, @this.Length)) + postfix;
        }

        public static string SubstringFromChar(this string @this, char chr)
        {
            var index = @this.IndexOf(chr);
            return @this.Substring(Math.Max(0, index));
        }

        public static string SubstringFromString(this string @this, string str)
        {
            var index = @this.IndexOf(str, StringComparison.CurrentCultureIgnoreCase);
            return @this.Substring(Math.Max(0, index));
        }

        public static string ToStringOrNull(this object obj)
        {
            return obj == null ? null : obj.ToString();
        }        

        public static string RemoveWhiteSpaces(this string @this)
        {
            return @this == null ? null : WhitespaceRegex.Replace(@this.Trim(), " ");
        }

        public static string[] Words(this string @this)
        {
            return @this?.RemoveWhiteSpaces().Split(' ');
        }

        public static string FirstCharToLower(this string str)
        {
            return char.ToLowerInvariant(str[0]) + str.Substring(1);
		}
		
        public static string GetBase64String(this string @this)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(@this));
        }

        public static string GetStringFromBase64String(this string @this)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String((@this)));
        }

        public static string ConcatWith(this string left, string leftSuffix, string rightPrefix, string right)
        {
            if (!string.IsNullOrWhiteSpace(right))
            {
                if (!string.IsNullOrWhiteSpace(left))
                {
                    left += leftSuffix;
                }
                left += rightPrefix + right;
            }
            return left;
        }

        /// <summary>
        /// Возвращает строку наибольшей длины с которой начинаются все строки items.
        /// </summary>
        /// <param name="items"></param>
        /// <returns>Null if items is empty;</returns>
        public static string CommonStarts(this IEnumerable<string> items)
        {
            string res = null;
            int minInd = 0;
            foreach (var str in items)
            {
                if (res == null)
                {
                    res = str;
                    minInd = res.Length;
                }
                else
                {
                    var ind = 0;
                    while (minInd > ind && str.Length > ind && str[ind] == res[ind])
                        ind++;
                    minInd = ind;

                }
            }
            return res.IfNotNull(x => x.Substring(0, minInd));
        }

        public static Tuple<int, int> ToFraction(this string pattern)
        {
            if (pattern == "1")
            {
                return new Tuple<int, int>(1, 1);
            }
            var fraction = pattern.Split('/');
            int numerator;
            int denominator;
            if (fraction.Length != 2 || !int.TryParse(fraction[0], out numerator) || !int.TryParse(fraction[1], out denominator))
            {
                throw new FormatException("Некорректный формат дроби" + pattern);
            }

            return new Tuple<int, int>(numerator, denominator);
        }

        public static string ReplaceInvalidFileNameChars(string path, string replacement = "")
        {
            var regex = new Regex(string.Format("[{0}]", Regex.Escape(new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars()))));
            return regex.Replace(path, replacement);
        }

        public static string ConcatenateStringArray(IEnumerable<string> parts, string separator)
        {
            return string.Join(separator, parts.Where(e => !string.IsNullOrWhiteSpace(e)));
        }

        private static readonly Regex WhitespaceRegex = new Regex("\\s+", RegexOptions.Multiline);
        private const string HtmlTagPattern = "<.*?>";
    }
}