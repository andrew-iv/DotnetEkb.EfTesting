using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DotnetEkb.EfTesting.Tests.Helpers.ExpressionHelpers
{
    public static class ExpressionHelper
    {
        private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new
            {
                f,
                s = second.Parameters[i]
            }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }

        public static Expression<Func<T, bool>> AndAll<T>(this IEnumerable<Expression<Func<T, bool>>> list)
        {
            if (!list.Any())
            {
                return x => true;
            }
            return list.Aggregate((current, next) => current.And(next));
        }

        public static Expression<Func<T, bool>> OrAll<T>(this IEnumerable<Expression<Func<T, bool>>> list)
        {
            if (!list.Any())
            {
                return x => true;
            }
            return list.Aggregate((current, next) => current.Or(next));
        }

        public static Expression TransformGenericMethod(this Expression node, params MethodInfo[] methodsInfo)
        {
            if (node == null) return null;
            var methodCallExpression = GetGenericMethodCallExpression(node);
            if (methodCallExpression == null)
                return node;
            var openedGenericMethod = methodCallExpression.Method.GetGenericMethodDefinition();
            if (methodsInfo.Any(mi => mi == openedGenericMethod))
                return Expression.Constant(Expression.Lambda(methodCallExpression).Compile().DynamicInvoke());
            return node;
        }

        private static MethodCallExpression GetGenericMethodCallExpression(Expression node)
        {
            if (node?.NodeType != ExpressionType.Call)
                return null;
            var result = node as MethodCallExpression;
            if (result == null)
                return null;
            return !result.Method.IsGenericMethod ? null : result;
        }
    }
}