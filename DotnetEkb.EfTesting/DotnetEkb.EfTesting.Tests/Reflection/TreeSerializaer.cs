using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotnetEkb.EfTesting.Tests.Reflection
{
    public class TreeSerializaer
    {
        protected virtual Func<PropertyInfo, bool> IgnoreRecursionProperty { get; private set; }
        protected virtual Func<Type, bool> IgnoreRecursionType { get; private set; }
        protected virtual Func<PropertyInfo, bool> IgnoreSerializationProperty { get; private set; }
        protected virtual Func<Type, bool> IgnoreSerializationType { get; private set; }

        public TreeSerializaer(ITreeSerializerSettings settings)
        {
            IgnoreRecursionType = settings.IgnoreRecursionType ?? (x => false);
            IgnoreSerializationType = settings.IgnoreSerializationType ?? (x => false);
            IgnoreRecursionProperty = settings.IgnoreRecursionProperty ?? (x => false);
            IgnoreSerializationProperty = settings.IgnoreSerializationProperty ?? (x => false);
        }

        public IEnumerable<KeyValuePair<string, object>> GetSerializationTree<TType>(object obj, string prefix = "")
        {
            return GetSerializationTree(obj, typeof (TType), new HashSet<object>(), prefix);
        }

        private IEnumerable<KeyValuePair<string, object>> GetSerializationTree(object obj, Type type, ISet<object> antiCycling, string prefix)
        {
            if (obj == null)
            {
                yield break;
            }
            if (IgnoreSerializationType.Invoke(type))
            {
                yield break;
            }
            if (IgnoreRecursionType.Invoke(type) || type == typeof (string) || type.IsValueType)
            {
                yield return new KeyValuePair<string, object>(prefix, obj);
                yield break;
            }
            foreach (var property in
                    type.GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance).Where(x => x != null && x.CanRead))
            {
                var value = property.GetValue(obj);
                var pt = property.PropertyType;
                if (IgnoreSerializationType.Invoke(pt) || IgnoreSerializationProperty(property))
                {
                    continue;
                }
                if (IgnoreRecursionProperty.Invoke(property) || IgnoreRecursionType.Invoke(pt) || pt == typeof (string) || pt.IsValueType)
                {
                    yield return new KeyValuePair<string, object>(string.Format("{0}{1}", prefix, property.Name), value);
                }
                else
                {
                    if (value == null || antiCycling.Contains(value))
                    {
                        continue;
                    }
                    if (typeof (IEnumerable).IsAssignableFrom(pt))
                    {
                        var enumType =
                                pt.GetInterfaces().Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof (IEnumerable<>)).ToArray().Select(t => t.GenericTypeArguments[0]).FirstOrDefault
                                        ();
                        if (enumType == null)
                        {
                            enumType = typeof (object);
                        }
                        antiCycling.Add(value);
                        var i = 0;
                        foreach (var item in (value as IEnumerable))
                        {
                            foreach (var val in
                                    GetSerializationTree(item, enumType, antiCycling, string.Format("{0}{1}[{2}].", prefix, property.Name, i++)))
                            {
                                yield return val;
                            }
                        }
                        antiCycling.Remove(value);
                    }
                    else
                    {
                        antiCycling.Add(value);
                        foreach (var val in GetSerializationTree(value, pt, antiCycling, string.Format("{0}{1}.", prefix, property.Name)))
                        {
                            yield return val;
                        }
                        antiCycling.Remove(value);
                    }
                }
            }
        }
    }
}