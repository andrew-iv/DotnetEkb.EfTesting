using System;
using System.Collections.Generic;
using System.Globalization;
using DotnetEkb.EfTesting.Tests.Helpers.BaseTypeHelpers;

namespace DotnetEkb.EfTesting.Tests.Helpers.DateHelpers
{
    public static class DateHelper
    {
        /// <summary>
        ///     Количество дней в неделе
        /// </summary>
        public const int DaysInWeek = 7;

        /// <summary>
        ///     Количество месяцев в году
        /// </summary>
        public const int MonthsInYear = 12;

        /// <summary>
        ///     <para>Строка форматирования даты по умолчанию</para>
        /// </summary>
        public const string DefaultDateFormat = "dd.MM.yyyy";

        /// <summary>
        ///     <para>Строка форматирования даты и времени по умолчанию</para>
        /// </summary>
        public const string DefaultDateTimeFormat = "dd.MM.yyyy HH:mm:ss";

        /// <summary>
        ///     <para>Строка форматирования даты и времени без секунд</para>
        /// </summary>
        public const string ShortDateTimeFormat = "dd.MM.yyyy HH:mm";

        /// <summary>
        ///     <para>Строка форматирования времени по умолчанию</para>
        /// </summary>
        public const string DefaultTimeFormat = "HH:mm:ss";

        /// <summary>
        ///     <para>Строка форматирования месяца и года по умолчанию</para>
        /// </summary>
        public const string DefaultMonthFormat = "y";

        /// <summary>
        ///     <para>Строка форматирования времени по умолчанию</para>
        /// </summary>
        public const string TimeFormatWithoutSeconds = @"hh\:mm";

        /// <summary>
        ///     <para>Строка форматирования времени gYearMonth</para>
        /// </summary>
        public const string GYearMonthFormat = @"yyyy-MM";

        /// <summary>
        ///     Максимальная дата типа smalldatetime 06.06.2079
        /// </summary>
        public static DateTime MaxSmallDateTime = new DateTime(2079, 6, 6);

        /// <summary>
        ///     Минимальная дата типа smalldatetime 01.01.1900
        /// </summary>
        public static DateTime MinSmallDateTime = new DateTime(1900, 1, 1);

        public static readonly string[] DayShortNames = {"Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс"};

        /// <summary>
        ///     Месяца в предложном падеже
        /// </summary>
        private static readonly string[] MonthInPrepositional =
        {
            "Январе", "Феврале", "Марте", "Апреле", "Мае", "Июне",
            "Июле", "Августе", "Сентябре", "Октябре", "Ноябре", "Декабре"
        };

        /// <summary>
        ///     Месяца в родительном падеже
        /// </summary>
        private static readonly string[] MonthInGenetive =
        {
            "Января", "Февраля", "Марта", "Апреля", "Мая", "Июня",
            "Июля", "Августа", "Сентября", "Октября", "Ноября", "Декабря"
        };

        /// <summary>
        ///     Относительные названия дней (смещение -> название)
        /// </summary>
        private static readonly Dictionary<int, string> RelativeDateNames = new Dictionary<int, string>
        {
            {0, "Сегодня"},
            {1, "Завтра"},
            {2, "Послезавтра"}
        };

        /// <summary>
        ///     Возвращает минимальную дату, DateTime с null значением считает максимальной датой
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static DateTime? Min(DateTime? a, DateTime? b)
        {
            if (a == null)
            {
                ObjectHelper.Swap(ref a, ref b);
            }
            if (a == null)
            {
                return null;
            }
            if (b == null)
            {
                return a;
            }
            return a < b ? a : b;
        }

        /// <summary>
        ///     Возвращает максимальную дату, DateTime с null значением считает максимальной датой
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static DateTime? Max(DateTime? a, DateTime? b)
        {
            if (a == null || b == null)
            {
                return null;
            }
            return a < b ? b : a;
        }

        public static DateTime PeriodEndDate(this DateTime date)
        {
            return date.Date.AddMonths(1).AddDays(-1);
        }

        public static string ToYearMonthType(this DateTime date)
        {
            return date.ToString("yyyy-MM");
        }

        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString(DefaultDateFormat);
        }

        public static string ToTimeString(this DateTime dateTime)
        {
            return dateTime.ToString(DefaultTimeFormat);
        }

        public static string ToDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString(DefaultDateTimeFormat);
        }

        public static string ToShortDateTimeFormat(this DateTime dateTime)
        {
            return dateTime.ToString(ShortDateTimeFormat);
        }

        public static DateTime ToXmlDateTimeFormat(this DateTime dateTime)
        {
            dateTime = dateTime.AddTicks(-(dateTime.Ticks%TimeSpan.TicksPerSecond));
            return dateTime.ToUniversalTime();
        }

        public static DateTime FromYearMonthType(string date)
        {
            if (date == null)
            {
                throw new ArgumentNullException();
            }

            var parts = date.Split('-');
            int year, month;
            if (parts.Length < 2 || !int.TryParse(parts[0], out year) || !int.TryParse(parts[1], out month))
            {
                throw new ArgumentException("Ожидалась дата в формате yyyy-MM.", "date");
            }

            if (month < 1 || month > 12)
            {
                throw new ArgumentException("Месяц должен быть числом от 1 до 12.", "date");
            }
            return new DateTime(year, month, 1);
        }

        public static bool TryParseYearMonthType(string period, out DateTime? date)
        {
            try
            {
                date = FromYearMonthType(period);
                return true;
            }
            catch (Exception)
            {
                date = null;
                return false;
            }
        }

        public static int GetDayOfWeek(this DateTime date)
        {
            var result = (int) date.DayOfWeek;
            if (result == 0)
            {
                result = 7;
            }
            return result;
        }

        public static string GetDayShortName(this DateTime date)
        {
            var dayOfWeek = GetDayOfWeek(date);
            return DayShortNames[dayOfWeek - 1];
        }

        public static bool IsHoliday(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday;
        }

        //public static IEnumerable<SelectListItem> GetMonthList()
        //{
        //    return Enumerable.Range(0, 12).Select(month => new SelectListItem
        //    {
        //        Text = DateTimeFormatInfo.CurrentInfo.MonthNames[month],
        //        Value = (month + 1).ToString()
        //    });
        //}

        public static string GetMonthName(int month)
        {
            if (month < 1 || month > 12)
            {
                throw new ArgumentException("Месяц должен быть в диапазоне от 1 до 12.", "month");
            }

            return DateTimeFormatInfo.CurrentInfo.MonthNames[month - 1];
        }

        public static string GetMonthGenitiveName(int month)
        {
            if (month < 1 || month > 12)
            {
                throw new ArgumentException("Месяц должен быть в диапазоне от 1 до 12.", "month");
            }

            return DateTimeFormatInfo.CurrentInfo.MonthGenitiveNames[month - 1];
        }

        public static string GetInPrepositional(this DateTime? dateNullable)
        {
            if (!dateNullable.HasValue)
            {
                return "";
            }
            var date = dateNullable.Value;
            return string.Format("В {0} {1}", MonthInPrepositional[date.Month - 1].ToLower(), date.Year);
        }
        

        public static string ToPeriod(this DateTime dateStart, DateTime? dateEnd)
        {
            var from = string.Format("с {0}", dateStart.ToString(DefaultDateFormat));
            return dateEnd.HasValue
                ? string.Format("{0} по {1}", @from, dateEnd.Value.ToString(DefaultDateFormat))
                : @from;
        }
        
        public static DateTime GetStartOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0, 0, dateTime.Kind);
        }

        public static DateTime GetStartOfYear(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 1, 1, 0, 0, 0, dateTime.Kind);
        }

        public static DateTime GetStartOfDay(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0, dateTime.Kind);
        }

        public static DateTime GetEndOfDay(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999, dateTime.Kind);
        }

        public static DateTime GetEndOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month), 23,
                59, 59, 999, dateTime.Kind);
        }

        public static DateTime GetEndOfYear(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 12, 31, 23, 59, 59, dateTime.Kind);
        }

        public static DateTime GetReportPeriod(this DateTime dateTime)
        {
            var minusMonth = dateTime.AddMonths(-1);
            return new DateTime(minusMonth.Year, minusMonth.Month, 1);
        }

        public static DateTime PreviousYear(this DateTime dateTime)
        {
            return dateTime.AddYears(-1);
        }

        public static DateTime PreviousMonth(this DateTime dateTime)
        {
            return dateTime.AddMonths(-1);
        }

        public static DateTime PreviousDay(this DateTime dateTime)
        {
            return dateTime.AddDays(-1);
        }

        public static DateTime NextYear(this DateTime dateTime)
        {
            return dateTime.AddYears(1);
        }

        public static DateTime NextMonth(this DateTime dateTime)
        {
            return dateTime.AddMonths(1);
        }

        public static DateTime NextDay(this DateTime dateTime)
        {
            return dateTime.AddDays(1);
        }

        public static bool IsSameDate(this DateTime self, DateTime other)
        {
            return self.Year == other.Year && self.Month == other.Month && self.Day == other.Day;
        }

        public static bool IsSameTime(this DateTime self, DateTime other)
        {
            return self.Hour == other.Hour && self.Minute == other.Minute && self.Second == other.Second;
        }

        public static string ToYearString(this DateTime date)
        {
            return string.Format("{0} г", date.Year);
        }

        public static IEnumerable<DateTime> RangeDays(this DateTime from, DateTime to)
        {
            var dates = new List<DateTime>();

            var date = @from;
            while (date <= to)
            {
                dates.Add(date);
                date = date.AddDays(1);
            }

            return dates;
        }

        public static string ToDateIntervalStandardFormat(this DateTime dateFrom, DateTime? dateTo,
            bool toShortView = false)
        {
            if (toShortView && dateTo.HasValue)
            {
                var isYearEqual = dateFrom.Year == dateTo.Value.Year;
                var isMonthEqual = dateFrom.Month == dateTo.Value.Month;
                var isRigthDays = dateFrom.PreviousDay().Month + 2 == dateTo.Value.NextDay().Month;
                if (isYearEqual && isMonthEqual && isRigthDays)
                {
                    return string.Format("{0}", dateFrom.ToString(DefaultMonthFormat));
                }
            }

            var interval = string.Format("с {0}", dateFrom.ToString(DefaultDateFormat));
            if (dateTo.HasValue)
                interval += string.Format(" по {0}", dateTo.Value.ToString(DefaultDateFormat));
            return interval;
        }

        public static string GetGenetiveDate(this DateTime date)
        {
            var day = date.Day;
            return string.Format("{0} {1}", day, MonthInGenetive[date.Month - 1].ToLower());
        }

        public static string GetGenetiveMonth(this DateTime date)
        {
            return MonthInGenetive[date.Month - 1].ToLower();
        }

        public static string ToTimeString(this TimeSpan time)
        {
            return time.ToString(TimeFormatWithoutSeconds);
        }

        public static string ToShortTimeString(this DateTime time)
        {
            return time.ToString(TimeFormatWithoutSeconds);
        }

        public static string ToGYearMonthString(this DateTime date)
        {
            return date.ToString(GYearMonthFormat);
        }

        public static string ToRelativeDate(this DateTime date)
        {
            foreach (var relativeDateName in RelativeDateNames)
            {
                if (date == DateTime.Today.AddDays(relativeDateName.Key))
                {
                    return string.Format("{0} ({1})", relativeDateName.Value, date.ToDateString());
                }
            }
            return date.ToDateString();
        }

        public static int IntervalsCount(DateTime from, DateTime to, int intervalLength)
        {
            if (intervalLength < 0 || @from > to)
                return 0;
            var dayDiff = (int) ((to - @from).TotalDays + 1);
            return dayDiff/intervalLength;
        }

        public static bool IsValidDate(int year, int month, int day)
        {
            return day >= 1 && month >= 1 && month <= 12 && DateTime.DaysInMonth(year, month) >= day;
        }
        
        public static int GetCalendarDaysCountFromToday(this DateTime date)
        {
            return (DateTime.Today - date.GetStartOfDay()).Days;
        }

        public static DateTime Max(DateTime a, DateTime b)
        {
            return a >= b ? a : b;
        }

        public static DateTime? MinWithNullEqInfinity(DateTime a, DateTime? b)
        {
            if (b == null)
                return a;
            return a <= b ? a : b;
        }

        public static int EndDateCompare(this DateTime? x, DateTime? y)
        {
            return new EndDateComparer().Compare(x, y);
        }
    }
}