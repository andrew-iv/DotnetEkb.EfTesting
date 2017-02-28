using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DotnetEkb.EfTesting.Tests.Helpers.EnumHelpers
{
    public static class EnumHelper
    {
        public static IEnumerable<T> Values<T>()
        {
            return Enum.GetValues(typeof (T)).Cast<T>();
        }

        public static string Description(this IConvertible value)
        {
            var attributes = value.GetAttributes<DescriptionAttribute>().ToArray();
            return attributes.Length > 0 ? attributes[0].Description : null;
        }

        public static IEnumerable<T> GetAttributes<T>(this IConvertible value)
            where T:Attribute
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attributes = (IEnumerable<T>)fieldInfo.GetCustomAttributes(typeof(T), false);
            return attributes;
        }

        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof (T);
            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof (DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                    {
                        return (T) field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
                        return (T) field.GetValue(null);
                    }
                }
            }
            throw new ArgumentException("Не найден элемент в перечислении с заданным description", "description");
        }

        public static SortedDictionary<string, string> GetBoundEnumDescriptionNameMap(this Type type)
        {
            if (!type.IsSubclassOf(typeof (Enum)))
            {
                throw new ArgumentException("T должен быть перечислением(Enum).");
            }
            var values = Enum.GetValues(type);
            var results = new SortedDictionary<string, string>();

            foreach (var value in values)
            {
                results.Add(Enum.GetName(type, value), Description((Enum) value));
            }
            return results;
        }
        
        public static int ToInt<TEnum>(this TEnum enumeration) where TEnum : struct
        {
            return (int) (ValueType) enumeration;
        }

        private static SortedDictionary<int, string> GetBoundEnumDescriptionMap<T>() where T : struct, IConvertible
        {
            if (!typeof (T).IsEnum)
            {
                throw new ArgumentException("T должен быть перечислением(Enum).");
            }
            var values = Enum.GetValues(typeof (T));
            var results = new SortedDictionary<int, string>();
            foreach (var value in values)
            {
                results.Add(Convert.ToInt32(value), ((Enum) value).Description());
            }
            return results;
        }

        private static IEnumerable<KeyValuePair<string, string>> GetBoundEnumDescriptionNameMap<T>() where T : struct, IConvertible
        {
            if (!typeof (T).IsEnum)
            {
                throw new ArgumentException("T должен быть перечислением(Enum).");
            }
            var values = Enum.GetValues(typeof (T));
            var results = new Dictionary<string, string>();

            foreach (var value in values)
            {
                results.Add(Enum.GetName(typeof (T), value), ((Enum) value).Description());
            }
            return results;
        }

        public static bool IsOneOf<TEnum>(this TEnum value, params TEnum[] variants) where TEnum : struct
        {
            return variants.Contains(value);
        }

        public static TEnum ForceParseEnum<TEnum>(this string enumValue) where TEnum : struct, IConvertible
        {
            if (string.IsNullOrEmpty(enumValue) || !typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("Type given must be an Enum", enumValue);
            }
            TEnum value;
            Enum.TryParse(enumValue, out value);
            return value;
        }

        public static TEnum? TryParseEnum<TEnum>(this string enumValue) where TEnum : struct, IConvertible
        {
            if (string.IsNullOrEmpty(enumValue) || !typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("Type given must be an Enum", enumValue);
            }
            TEnum enumVar;
            var successParse = Enum.TryParse(enumValue, out enumVar);
            if (!successParse)
            {
                return null;
            }
            return enumVar;
        }

        public static TEnum ParseEnum<TEnum>(this string enumValue) where TEnum : struct, IConvertible
        {
            if (string.IsNullOrEmpty(enumValue) || !typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("Type given must be an Enum", enumValue);
            }
            return (TEnum)Enum.Parse(typeof(TEnum), enumValue);
        }
    }

    public static class Enum<T>
    {
        public static T Parse(string value)
        {
            return (T) Enum.Parse(typeof (T), value, true);
        }

        public static string GetDescription(string value)
        {
            return ((Enum) Enum.Parse(typeof (T), value, true)).Description();
        }
    }
}