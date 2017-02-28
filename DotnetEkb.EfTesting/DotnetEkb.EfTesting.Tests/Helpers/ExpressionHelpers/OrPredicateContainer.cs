using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DotnetEkb.EfTesting.Tests.Helpers.ExpressionHelpers
{
    public class OrPredicateContainer<T>
    {
        private readonly List<Expression<Func<T, bool>>> _list = new List<Expression<Func<T, bool>>>();

        public OrPredicateContainer<T> Or(Expression<Func<T, bool>> predicate)
        {
            _list.Add(predicate);
            return this;
        }

        /// <summary>
        /// Сравнивает с дефолтным значением. И добавляет к expression, если значение недефолтное
        /// </summary>
        public OrPredicateContainer<T> OrIf<TFilter>(TFilter filterProperty, Expression<Func<T, bool>> predicate)
        {
            if (!Equals(filterProperty, default(TFilter)))
                _list.Add(predicate);
            return this;
        }

        public Expression<Func<T, bool>> Predicate => _list.OrAll();
    }
}