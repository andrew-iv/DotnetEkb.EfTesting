namespace DotnetEkb.EfTesting.Tests.Helpers.ConvertHelpers
{
    public static class BoolConvertHelper
    {
        public static string ToYesOrNoString(this bool value)
        {
            return value ? "Да" : "Нет";
        }

        public static string ToJsString(this bool value)
        {
            return value.ToString().ToLower();
        }
    }

    public class BooleanWithDefault
    {
        public bool IsSpecified { get; set; }
        public bool? Value { get; set; }

        public static implicit operator bool?(BooleanWithDefault value)
        {
            if (value == null)
            {
                return null;
            }
            return !value.IsSpecified ? null : value.Value;
        }

        public static implicit operator BooleanWithDefault(bool? value)
        {
            return new BooleanWithDefault
            {
                IsSpecified = true,
                Value = value
            };
        }
    }
}