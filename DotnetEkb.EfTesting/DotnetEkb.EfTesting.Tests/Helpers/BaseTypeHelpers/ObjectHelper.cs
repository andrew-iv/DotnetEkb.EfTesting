using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace DotnetEkb.EfTesting.Tests.Helpers.BaseTypeHelpers
{
    public static class ObjectHelper
    {
        /// <summary>
        /// Выполяняет выражение если параметер не null.
        /// <b>ОСТОРОЖНО с кастованием valueType к nullable <br/>
        /// например default(int) = 0, нужно непосредственно указывать в TRes int?</b>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TRes">ОСТОРОЖНО с nullable</typeparam>
        /// <param name="type"></param>
        /// <param name="func"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TRes IfNotNull<T, TRes>(this T type, Func<T, TRes> func, TRes defaultValue = default(TRes))
        {
            if (type != null)
            {
                return func(type);
            }
            return defaultValue;
        }

        public static void IfNotNullVoid<T>(this T type, Action<T> func)
        {
            if (type != null)
            {
                func(type);
            }
        }

        public static void ThrowIfNotNull<T>(this T type, Func<T, Exception> constructor)
        {
            if (type != null)
            {
                throw constructor(type);
            }
        }

        public static TRes IfNotNull<T, TRes>(this T type, Func<T, TRes> func, Func<TRes> defaultValue)
        {
            if (type != null)
            {
                return func(type);
            }
            return defaultValue();
        }

        public static object GetDefault(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        public static object GetProperty(this object obj, string name)
        {
            return obj.GetType().GetProperty(name).GetValue(obj, null);
        }

        public static bool Swap<T>(ref T x, ref T y)
        {
            try
            {
                var t = y;
                y = x;
                x = t;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool EqualsNull(this object obj1, object obj2)
        {
            if (obj1 == null & obj2 == null)
            {
                return true;
            }
            if (obj1 == null)
            {
                return false;
            }
            return obj1.Equals(obj2);
        }

        public static List<KeyValuePair<string, object>> GetKeyValueCollection(this object obj, IEnumerable<string> keys = null, bool includeDefault = false, bool includeReadonly = false)
        {
            var type = obj.GetType();
            IEnumerable<PropertyInfo> query = type.GetProperties();
            if (!includeReadonly)
            {
                query = query.Where(x => x.CanWrite);
            }
            if (!includeDefault)
            {
                query = query.Where(x => x.GetValue(obj, null) != null && !x.GetValue(obj, null).Equals(x.PropertyType.GetDefault()));
            }

            if (keys != null)
            {
                var set = new HashSet<string>(keys);
                query = query.Where(x => set.Contains(x.Name));
            }
            return query.Select(x => new KeyValuePair<string, object>(x.Name, x.GetValue(obj, null))).ToList();
        }

        /// <summary>
        /// Converts object to expando.
        /// See http://blog.jorgef.net/2011/06/converting-any-object-to-dynamic.html for details
        /// </summary>
        public static ExpandoObject ToExpando(this object value)
        {
            if (value == null)
                return null;
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                expando.Add(property.Name, property.GetValue(value));

            return (ExpandoObject) expando;
        }
    }
}