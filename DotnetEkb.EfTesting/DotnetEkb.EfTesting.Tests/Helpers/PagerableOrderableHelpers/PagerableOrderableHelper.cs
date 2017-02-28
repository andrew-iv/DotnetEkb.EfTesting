using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DotnetEkb.EfTesting.Tests.Helpers.PagerableOrderableHelpers
{
    public static class PagerableOrderableHelper
    {
        /// <summary>
        /// Расширение для постраничного вывода коллекции
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Коллекция</param>
        /// <param name="page">Номер страницы</param>
        /// <param name="pageSize">Количество элементов на странице</param>
        /// <param name="objectsCount">Общее количество объектов в коллекции</param>
        /// <returns></returns>
        public static IQueryable<T> Page<T>(this IOrderedQueryable<T> obj, int page, int pageSize, out int objectsCount)
        {
            objectsCount = obj.Count();
            if (page > 1 && (page - 1) * pageSize >= objectsCount)
            {
                page = (objectsCount - 1) / pageSize + 1;
            }
            return obj.Page(page, pageSize);
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> obj, int page, int pageSize, out int objectsCount)
        {
            objectsCount = obj.Count();
            return obj.Page(page, pageSize);
        }

        /// <summary>
        /// Расширение для постраничного вывода коллекции
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Коллекция</param>
        /// <param name="page">Номер страницы</param>
        /// <param name="pageSize">Количество элемнтов на странице</param>
        /// <param name="asc">Если true - применяется сортировка по возрастанию, иначе по убыванию</param>
        /// <returns></returns>
        public static IQueryable<T> Page<T>(this IOrderedQueryable<T> obj, int page, int pageSize)
        {
            if (page < 1)
            {
                page = 1;
            }
            var skipParam = (page - 1) * pageSize;
            return obj.Skip(skipParam).Take(pageSize) as IQueryable<T>;
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> obj, int page, int pageSize)
        {
            if (page < 1)
            {
                page = 1;
            }
            var skipParam = (page - 1) * pageSize;
            return obj.Skip(skipParam).Take(pageSize) as IQueryable<T>;
        }

        /// <summary>
        /// Расширение для постраничного вывода коллекции
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="obj">Коллекция</param>
        /// <param name="page">Номер страницы</param>
        /// <param name="pageSize">Количество элемнтов на странице</param>
        /// <param name="keySelector">Свойство для сортировки</param>
        /// <param name="asc">Если true - применяется сортировка по возрастанию, иначе по убыванию</param>
        /// <returns></returns>
        public static IQueryable<T> Page<T, TResult>(this IQueryable<T> obj, int page, int pageSize, Expression<Func<T, TResult>> keySelector, bool asc)
        {
            if (page < 1)
            {
                page = 1;
            }
            var skipParam = (page - 1) * pageSize;
            return asc ? obj.OrderBy(keySelector).Skip(skipParam).Take(pageSize) : obj.OrderByDescending(keySelector).Skip(skipParam).Take(pageSize);
        }

        /// <summary>
        /// Расширение для постраничного вывода коллекции
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="obj">Коллекция</param>
        /// <param name="page">Номер страницы</param>
        /// <param name="pageSize">Количество элемнтов на странице</param>
        /// <param name="keySelector">Свойство для сортировки</param>
        /// <param name="asc">Если true - применяется сортировка по возрастанию, иначе по убыванию</param>
        /// <param name="objectsCount">Общее количество объектов в коллекции</param>
        /// <returns></returns>
        public static IQueryable<T> Page<T, TResult>(this IQueryable<T> obj, int page, int pageSize, Expression<Func<T, TResult>> keySelector, bool asc, out int objectsCount)
        {
            objectsCount = obj.Count();
            if (page < 1)
            {
                page = 1;
            }
            var skipParam = (page - 1) * pageSize;
            return asc ? obj.OrderBy(keySelector).Skip(skipParam).Take(pageSize) : obj.OrderByDescending(keySelector).Skip(skipParam).Take(pageSize);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, false, false);
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, true, false);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool asc)
        {
            return OrderingHelper(source, propertyName, !asc, false);
        }

        public static IOrderedQueryable<T> OrderBy<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> expr, bool asc)
        {
            return asc ? source.OrderBy(expr) : source.OrderByDescending(expr);
        }

        public static IOrderedQueryable<T> ThenBy<T, TKey>(this IOrderedQueryable<T> source, Expression<Func<T, TKey>> expr, bool asc)
        {
            return asc ? source.ThenBy(expr) : source.ThenByDescending(expr);
        }

        public static IOrderedQueryable<T> OrderThenByAsc<T, TKey>(this IOrderedQueryable<T> source, Expression<Func<T, TKey>> expr, bool asc)
        {
            return asc ? source.ThenBy(expr) : source.ThenByDescending(expr);
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, false, true);
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, true, true);
        }

        private static IOrderedQueryable<T> OrderingHelper<T>(IQueryable<T> source, string propertyName, bool descending, bool anotherLevel)
        {
            var param = Expression.Parameter(typeof(T), string.Empty);
            var property = Expression.PropertyOrField(param, propertyName);
            var sort = Expression.Lambda(property, param);

            var call = Expression.Call(typeof(Queryable), (!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : string.Empty), new[] { typeof(T), property.Type }, source.Expression,
                                       Expression.Quote(sort));

            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
        }

        private static readonly MethodInfo SkipMethodInfo =
        typeof(Queryable).GetMethod("Skip");

        public static IQueryable<TSource> Skip<TSource>(
            this IQueryable<TSource> source,
            Expression<Func<int>> countAccessor)
        {
            return Parameterize(SkipMethodInfo, source, countAccessor);
        }

        private static readonly MethodInfo TakeMethodInfo =
            typeof(Queryable).GetMethod("Take");

        public static IQueryable<TSource> Take<TSource>(
            this IQueryable<TSource> source,
            Expression<Func<int>> countAccessor)
        {
            return Parameterize(TakeMethodInfo, source, countAccessor);
        }

        private static IQueryable<TSource> Parameterize<TSource, TParameter>(
            MethodInfo methodInfo,
            IQueryable<TSource> source,
            Expression<Func<TParameter>> parameterAccessor)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (parameterAccessor == null)
                throw new ArgumentNullException("parameterAccessor");
            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    methodInfo.MakeGenericMethod(new[] { typeof(TSource) }),
                    new[] { source.Expression, parameterAccessor.Body }));
        }
    }
}