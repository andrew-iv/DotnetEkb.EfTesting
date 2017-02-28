using System;
using System.Globalization;

namespace DotnetEkb.EfTesting.Tests.Helpers.ConvertHelpers
{
    /// <summary>
    /// Helper конвертации типов.
    /// </summary>
    public static class ConvertersHelper
    {
        /// <summary>
        /// Пытаемся преобразовать входящее значение к типу decimal. Если входящий параметр - null возвращаем 0.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToNulableDecimal(object value)
        {
            return (value == null) ? 0 : ToDecimal(value);
        }

        /// <summary>
        /// Преобразование строки в шестнадцатеричном виде в массив байт 
        /// </summary>
        public static byte[] HexStringToByteArray(string hex)
        {
            var numberChars = hex.Length;
            var bytes = new byte[numberChars / 2];
            for (var i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        public static byte[] ToAsciiByteArray(string asciiString)
        {
            var result = new byte[asciiString.Length];
            int index = 0;
            foreach(var chr in asciiString)
            {
                result[index++] = (byte) chr;
            }
            return result;
        }

        /// <summary>
        /// Преобразование массива байт в строку в шестнадцатеричном виде 
        /// </summary>
        public static string BytesToHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        /// <summary>
        /// Получаем строковое представление месяца в родительном падеже для текущей даты
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string GetMonth(DateTime dateTime)
        {
            var month = dateTime.Month;
            if (month == 1)
            {
                return "января";
            }
            if (month == 2)
            {
                return "февраля";
            }
            if (month == 3)
            {
                return "марта";
            }
            if (month == 4)
            {
                return "апреля";
            }
            if (month == 5)
            {
                return "мая";
            }
            if (month == 6)
            {
                return "июня";
            }
            if (month == 7)
            {
                return "июля";
            }
            if (month == 8)
            {
                return "августа";
            }
            if (month == 9)
            {
                return "сентября";
            }
            if (month == 10)
            {
                return "октября";
            }
            if (month == 11)
            {
                return "ноября";
            }
            return "декабря";
        }

        /// <summary>
        /// Пытаемся преобразовать входящее значение к типу decimal.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static decimal ToDecimal(object value)
        {
            try
            {
                var cultureInfo = new CultureInfo("ru-RU");
                return Convert.ToDecimal(value, cultureInfo);
            }
            catch (Exception)
            {
                return Convert.ToDecimal(value);
            }
        }
    }
}