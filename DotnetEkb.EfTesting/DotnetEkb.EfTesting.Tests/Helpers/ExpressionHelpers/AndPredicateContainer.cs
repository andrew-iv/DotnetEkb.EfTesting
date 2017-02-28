using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DotnetEkb.EfTesting.Tests.Helpers.ExpressionHelpers
{
    public class AndPredicateContainer<T>
    {
        private readonly List<Expression<Func<T, bool>>> _list = new List<Expression<Func<T, bool>>>();

        public AndPredicateContainer<T> And(Expression<Func<T, bool>> predicate)
        {
            _list.Add(predicate);
            return this;
        }

        /// <summary>
        /// ���������� � ��������� ���������. � ��������� � expression, ���� �������� �����������
        /// </summary>
        public AndPredicateContainer<T> AndIf<TFilter>(TFilter filterProperty, Expression<Func<T, bool>> predicate)
        {
            if (!Equals(filterProperty, default(TFilter)))
                _list.Add(predicate);
            return this;
        }

        public Expression<Func<T, bool>> Predicate => _list.AndAll();

    }
}