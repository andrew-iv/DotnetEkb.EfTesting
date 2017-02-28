using System;
using System.Reflection;

namespace DotnetEkb.EfTesting.Tests.Reflection
{
    public interface ITreeSerializerSettings
    {
        Func<PropertyInfo, bool> IgnoreRecursionProperty { get; }
        Func<Type, bool> IgnoreRecursionType { get; }
        Func<PropertyInfo, bool> IgnoreSerializationProperty { get; }
        Func<Type, bool> IgnoreSerializationType { get; }
    }
}