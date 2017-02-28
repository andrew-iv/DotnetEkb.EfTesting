using System;
using System.Collections.Generic;
using System.Reflection;
using DotnetEkb.EfTesting.Tests.Reflection;

namespace DotnetEkb.EfTesting.Tests.Helpers.SerializeHelpers
{
    public static class SerializationHelper
    {
        public static IEnumerable<KeyValuePair<string, object>> GetSerializationTree<TType>(this object obj, string prefix = "", Func<Type, bool> ignoreRecursionType = null,
                Func<PropertyInfo, bool> ignoreRecursionProperty = null, Func<Type, bool> ignoreSerializationType = null, Func<PropertyInfo, bool> ignoreSerializationProperty = null)
        {
            var treeSerializaer = new TreeSerializaer(new TreeSerializerSettings
            {
                IgnoreRecursionType = ignoreRecursionType,
                IgnoreRecursionProperty = ignoreRecursionProperty,
                IgnoreSerializationType = ignoreSerializationType,
                IgnoreSerializationProperty = ignoreSerializationProperty
            });

            return treeSerializaer.GetSerializationTree<TType>(obj, prefix);
        }
    }
}