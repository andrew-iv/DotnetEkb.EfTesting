using System;
using System.Linq.Expressions;
using System.Reflection;
using DotnetEkb.EfTesting.Tests.Reflection.Impl;

namespace DotnetEkb.EfTesting.Tests.Reflection
{
    /// <summary>
    /// Источник копипаста http://automapper.org/
    /// </summary>
    public static class ReflectionHelper
    {
        public static MemberInfo FindProperty(LambdaExpression lambdaExpression)
        {
            Expression expressionToCheck = lambdaExpression;

            bool done = false;

            while (!done)
            {
                switch (expressionToCheck.NodeType)
                {
                    case ExpressionType.Convert:
                        expressionToCheck = ((UnaryExpression)expressionToCheck).Operand;
                        break;
                    case ExpressionType.Lambda:
                        expressionToCheck = ((LambdaExpression)expressionToCheck).Body;
                        break;
                    case ExpressionType.MemberAccess:
                        var memberExpression = ((MemberExpression)expressionToCheck);

                        if (memberExpression.Expression.NodeType != ExpressionType.Parameter &&
                            memberExpression.Expression.NodeType != ExpressionType.Convert)
                        {
                            throw new ArgumentException(string.Format("Expression '{0}' must resolve to top-level member and not any child object's properties. Use a custom resolver on the child type or the AfterMap option instead.", lambdaExpression), "lambdaExpression");
                        }

                        MemberInfo member = memberExpression.Member;

                        return member;
                    default:
                        done = true;
                        break;
                }
            }

            throw new ArgumentException("Only supported for top-level individual members on a type.");
        }

        public static IMemberAccessor<TObject, TProperty> CreatePropertyAccessor<TObject, TProperty>(Expression<Func<TObject, TProperty>> property)
        {
            var propertyInfo = FindProperty(property) as PropertyInfo;
            if(propertyInfo == null)
                throw new ArgumentException("Поддерживаются только свойства объекта", nameof(property));
            return new PropertyAccessor<TObject, TProperty>(propertyInfo);
        }
    }
}