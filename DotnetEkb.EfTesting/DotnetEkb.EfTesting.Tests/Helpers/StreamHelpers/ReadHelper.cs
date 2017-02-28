using System;
using System.IO;

namespace DotnetEkb.EfTesting.Tests.Helpers.StreamHelpers
{
    public static class ReadHelper
    {
		public static byte[] ReadAll(this Stream input)
		{
            using (var stream = new MemoryStream())
            {
                input.CopyTo(stream);
                return stream.ToArray();
            }
        }

        public static void PartitialyCopyTo(this Stream input, Stream output, int bytes)
        {
            byte[] buffer = new byte[32768];
            int read;
            while (bytes > 0 &&
                   (read = input.Read(buffer, 0, Math.Min(buffer.Length, bytes))) > 0)
            {
                output.Write(buffer, 0, read);
                bytes -= read;
            }
        }

        public static string ReadAllText(this Stream input)
        {
            using (var reader = new StreamReader(input))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
