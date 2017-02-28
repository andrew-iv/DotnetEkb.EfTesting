using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DotnetEkb.EfTesting.Tests.Reflection.Impl
{
    public static class DelegateFactory
    {
        public static Func<TObject,TProperty> CreateGet<TObject, TProperty>(PropertyInfo property)
        {
            ParameterExpression instanceParameter = Expression.Parameter(typeof(object), "target");

            MemberExpression member = Expression.Property(Expression.Convert(instanceParameter, property.DeclaringType), property);

            Expression<Func<TObject, TProperty>> lambda = Expression.Lambda<Func<TObject, TProperty>>(
                Expression.Convert(member, typeof(TProperty)),
                instanceParameter
                );

            return lambda.Compile();
        }

        public static Action<TSource, TDestination> CreateSet<TSource, TDestination>(PropertyInfo property)
        {
            return (target, value) => property.SetValue(target, value, null);
        }
        
    }


    public delegate object LateBoundPropertyGet(object target);
    public delegate void LateBoundPropertySet(object target, object value);

    public class PropertyAccessor<TObject, TResult> : IMemberAccessor<TObject, TResult>
    {
        private readonly Func<TObject, TResult> _lateBoundPropertyGet;
        private readonly Action<TObject, TResult> _lateBoundPropertySet;

        public PropertyAccessor(PropertyInfo propertyInfo)
        {
            if (propertyInfo.GetGetMethod(true) != null)
                _lateBoundPropertyGet = DelegateFactory.CreateGet<TObject, TResult>(propertyInfo);
            else
                _lateBoundPropertyGet = src => default(TResult);
            if (propertyInfo.GetSetMethod(true) != null)
            {
                _lateBoundPropertySet = DelegateFactory.CreateSet<TObject, TResult>(propertyInfo);
            }
            else
                _lateBoundPropertySet = (obj, res) => { };
        }

        public void SetValue(TObject destination, TResult value)
        {
            _lateBoundPropertySet(destination, value);
        }

        public TResult GetValue(TObject source)
        {
            return _lateBoundPropertyGet(source); ;
        }
    }
}