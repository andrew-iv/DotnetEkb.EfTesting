using System;
using System.Reflection;

namespace DotnetEkb.EfTesting.Tests.Reflection
{
    internal class TreeSerializerSettings : ITreeSerializerSettings
    {
        public Func<Type, bool> IgnoreRecursionType { get; set; }
        public Func<Type, bool> IgnoreSerializationType { get; set; }
        public Func<PropertyInfo, bool> IgnoreRecursionProperty { get; set; }
        public Func<PropertyInfo, bool> IgnoreSerializationProperty { get; set; }
    }
}