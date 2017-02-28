using System.Linq;

namespace DotnetEkb.EfTesting.Tests.Helpers
{
    public static class FioHelper
    {
        public static string GetShortFio(string lastName, string firstName, string middleName=null)
        {
            return string.IsNullOrEmpty(middleName)
                        ? $"{lastName} {firstName.First()}."
                        : $"{lastName} {firstName.First()}. {middleName.First()}.";
        }
    }
}
