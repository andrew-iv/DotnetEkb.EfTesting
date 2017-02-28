using System.Linq;
using System.Linq.Expressions;

namespace DotnetEkb.EfTesting.Tests.Helpers.ExpressionHelpers
{
    public static class QueryableHelper
    {
        /// <summary>
        /// Костыль для запросов по интерфейсу. Просто вызвать метод.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q"></param>
        /// <returns></returns>
        public static IQueryable<T> Fix<T>(this IQueryable<T> q)
        {
            var visitor = new FixupVisitor();
            return q.Provider.CreateQuery<T>(visitor.Visit(q.Expression));
        }

        internal class FixupVisitor : ExpressionVisitor
        {
            protected override Expression VisitUnary(UnaryExpression u)
            {
                if (u.NodeType != ExpressionType.Convert)
                {
                    return base.VisitUnary(u);
                }

                var operandType = u.Operand.Type;
                var expressionType = u.Type;
                if (expressionType.IsInterface && operandType.GetInterfaces().Contains(expressionType))
                {
                    return Visit(u.Operand);
                }
                return base.VisitUnary(u);
            }
        }
    }
}