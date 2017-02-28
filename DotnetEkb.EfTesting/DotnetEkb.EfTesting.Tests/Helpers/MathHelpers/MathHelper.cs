using System;

namespace DotnetEkb.EfTesting.Tests.Helpers.MathHelpers
{
    public static class MathHelper
    {
        public static int Between(int current, int min, int max)
        {
            return Math.Max(Math.Min(current, max), min);
        }
    }
}
