using System.Globalization;

namespace DotnetEkb.EfTesting.Tests.Helpers.FormattingHelpers
{
    public static class IntFormatHelper
    {
        public static string IntToFileSizeString(int fileSize)
        {
            var cultureInfo = CultureInfo.InvariantCulture;
            var formatInfo = (NumberFormatInfo) cultureInfo.NumberFormat.Clone();
            formatInfo.NumberGroupSeparator = " ";
            var sizeInKb = (fileSize + 1023) / 1024;
            var sizeInMb = (sizeInKb + 1023) / 1024;
            return sizeInKb >= 1024 ? (sizeInMb + " MB") : (sizeInKb + " KB");
        }
    }
}