namespace DotnetEkb.EfTesting.Tests.Helpers.BaseTypeHelpers
{
    public static class IntHelper
    {
        public static bool IsNullOrZero(this int? val)
        {
            return val == null || val == 0;
        }
    }
}
