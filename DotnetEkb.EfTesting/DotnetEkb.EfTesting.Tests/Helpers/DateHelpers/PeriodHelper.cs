using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DotnetEkb.EfTesting.Tests.Helpers.DateHelpers
{
    public static class PeriodHelper
    {
        public const int YearReportMonth = 3;

        public static DateTime FirstPeriod
        {
            get { return DateTime.Parse(ConfigurationSettings.AppSettings["StartPeriod"]); }
        }

        public static int GetPeriodId(this DateTime period)
        {
            var periods = GetPeriods();
            PeriodModel periodModel = periods.FirstOrDefault(x => x.Month == period.Month && x.Year == period.Year);
            if(periodModel == null)
                throw new ArgumentException("Не удается найти период для даты {0}", period.ToString("dd.MM.yyyy"));

            return periodModel.Id;
        }

        public static DateTime GetPeriod(this int periodId)
        {
            return FirstPeriod.AddMonths(periodId - 1);
        }

        public static IEnumerable<PeriodModel> GetPeriods(bool withoutLast = false)
        {
            var periods = new List<PeriodModel>();
            var value = ConfigurationSettings.AppSettings["StartPeriod"];

            var appDate = DateTime.Parse(value);
            var nowDate = withoutLast ? DateTime.Now.AddMonths(-1): DateTime.Now;
            var i = 1;
            while (appDate < nowDate)
            {
                periods.Add(new PeriodModel
                {
                    Id = i++,
                    Month = appDate.Month,
                    Year = appDate.Year
                });
                appDate = appDate.AddMonths(1);
            }
            return periods;
        }

        public static IEnumerable<DateTime> GetDatePeriods()
        {
            var periods = new List<DateTime>();
            var value = ConfigurationSettings.AppSettings["StartPeriod"];

            var date = DateTime.Parse(value);
            var now = DateTime.Now;
            while (date < now)
            {
                periods.Add(date);
                date = date.AddMonths(1);
            }
            return periods;
        }

        //public static IEnumerable<SelectListItem> GetSelectListItemPeriods( bool asc = false, DateTime? value = null )
        //{
        //    return GetPeriodsSelectList(asc ? GetDatePeriods().ToList() : GetDatePeriods().OrderByDescending(s => s).ToList(), value);
        //}

        //public static IEnumerable<SelectListItem> GetYearsList( int maxYear)
        //{
        //    var minYear = FirstPeriod.Year;
        //    for (var year = maxYear; year >= minYear; year--)
        //        yield return new SelectListItem { Text = year.ToString(), Value = year.ToString()};
        //}

        public static PeriodModel DateToPeriod(DateTime date)
        {
            return GetPeriods().ForDate(date);
        }

        public static bool IsFirstMonthInQuarter(DateTime period)
        {
            return period.Month % 3 == 1;
        }

        public static bool ForYearlyPlan(DateTime period)
        {
            return period.Month == 12 || period.Month <= 3;
        }

        //public static List<SelectListItem> GetPreviousAndCurrentPeriodSelectList(DateTime period)
        //{
        //    var previosPeriod = period.AddMonths(-1);
        //    var mounths = GetPeriodsSelectList(new List<DateTime>()
        //    {
        //        previosPeriod,
        //        period
        //    });
        //    mounths.AddSelection(CommonConsts.EmptySelection, CommonConsts.EmptySelectionValue.ToString());
        //    return mounths;
        //}

        //public static List<SelectListItem> GetPeriodsSelectList(List<DateTime> periods, DateTime? value = null)
        //{
        //    return periods.Select(q => new SelectListItem
        //    {
        //        Value = q.ToShortDateString(),
        //        Selected = value == q,
        //        Text = String.Format("{0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(q.Month), q.Year)
        //    }).ToList();
        //}

        public static string GetPlanningJobPeriodString(this DateTime period)
        {
            return period.ToString("yyyy.MM");
        }

        //public static DateTime GetPeriodDate(int? periodId)
        //{
        //    var periods = GetPeriods();
        //    var period = periods.Last();
            

        //    if (periodId != null && periodId != 0)
        //    {
        //        period = periods.FirstOrDefault(x => x.Id == periodId);
        //    }

        //    if (period == null)
        //    {
        //        throw new HttpResponseException(HttpStatusCode.BadRequest);
        //    }
        //    return new DateTime(period.Year, period.Month, 1);
        //}

        public static DateTime GetEndOfReportPeriod(this DateTime period)
        {
            return period.GetStartOfMonth().AddMonths(1).AddDays(-1).Date;
        }
    }

    public class PeriodModel
    {
        public DateTime DateTime
        {
            get { return new DateTime(Year, Month, 1); }
        }

        public int Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        
        public string ToPeriodYearString()
        {
            return string.Format("{0} г", this);
        }
    }

    //public static class PeriodsExtensions
    //{
    //    public static IEnumerable<SelectListItem> ToDatesSelectListItems(this IEnumerable<DateTime> dates)
    //    {
    //        return dates.Select(date => new SelectListItem
    //        {
    //            Text = date.ToDateString(),
    //            Value = date.ToString()
    //        }).ToArray();
    //    }

    //    public static IEnumerable<SelectListItem> ToPeriodsSelectListItems(this IEnumerable<DateTime> dates)
    //    {
    //        return dates.Select(date => new SelectListItem
    //        {
    //            Text = date.ToPeriodString(),
    //            Value = date.ToString()
    //        }).ToArray();
    //    }
    //}

    public static class PeriodModelExtensions
    {
        //public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<PeriodModel> periods)
        //{
        //    return periods.Select(period => new SelectListItem
        //    {
        //        Text = period.ToString(),
        //        Value = period.Id.ToString(CultureInfo.InvariantCulture)
        //    }).ToArray();
        //}

        public static PeriodModel ForDate(this IEnumerable<PeriodModel> periods, DateTime date)
        {
            return periods.FirstOrDefault(period => period.Year == date.Year && period.Month == date.Month);
        }

        public static PeriodModel Previous(this IEnumerable<PeriodModel> periods, PeriodModel current)
        {
            var first = periods.First();
            if (first.Year == current.Year && first.Month == current.Month)
            {
                return first;
            }

            var year = current.Year;
            var month = current.Month;
            if (month == 1)
            {
                year = year - 1;
                month = 12;
            }
            else
            {
                month = month - 1;
            }

            return periods.Single(period => period.Year == year && period.Month == month);
        }

        public static PeriodModel Next(this IEnumerable<PeriodModel> periods, PeriodModel current)
        {
            var last = periods.Last();
            if (last.Year == current.Year && last.Month == current.Month)
            {
                return last;
            }

            var year = current.Year;
            var month = current.Month;
            if (month == 12)
            {
                year = year + 1;
                month = 1;
            }
            else
            {
                month = month + 1;
            }

            return periods.Single(period => period.Year == year && period.Month == month);
        }

        public static bool HasPeriodIntersection(this DateTime periodStart, DateTime start, DateTime? end)
        {
            var endPeriod = periodStart.PeriodEndDate();
            return start <= endPeriod && (end == null || end >= periodStart);
        }

        public static bool IsBetween(this DateTime point, DateTime start, DateTime? end)
        {
            return start <= point && (end == null || end >= point);
        }

        public static SortedSet<int> IsPeriodsIntersectedIndexes<T, TType>(this IList<T> collection, Func<T, TType> start, Func<T, TType> end, IComparer<TType> comparer = null)
        {
            var res = new SortedSet<int>();
            comparer = comparer ?? Comparer<TType>.Default;
            int i = 0;
            var arr = collection.Select(x => new
            {
                ind = i++,
                st = start(x),
                end = end(x)
            }).OrderBy(x => x.st, comparer).ToArray();

            int nxt = 1;
            var barrier = arr.Length - 1;
            for (i = 0; i < barrier; i++)
            {
                if (nxt <= i)
                    nxt = i + 1;
                bool firstAdd = true;
                var lastEnd = arr[i].end;
                var st = arr[nxt].st;
                while ((lastEnd == null || st == null || comparer.Compare(st, lastEnd) <= 0))
                {
                    if (firstAdd)
                    {
                        res.Add(arr[i].ind);
                        firstAdd = false;
                    }
                    res.Add(arr[nxt].ind);
                    if (nxt == barrier)
                        break;
                    st = arr[++nxt].st;
                }
            }
            return res;
        }
    }
}