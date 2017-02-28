using System;
using System.IO;

namespace DotnetEkb.EfTesting.Tests.Helpers.ConvertHelpers
{
    public static class StreamConverter
    {
        public static byte[] ToByteArray(this Stream inputStream)
        {
            if (!inputStream.CanRead)
            {
                throw new ArgumentException();
            }

            // This is optional
            if (inputStream.CanSeek)
            {
                inputStream.Seek(0, SeekOrigin.Begin);
            }

            var output = new byte[inputStream.Length];
            inputStream.Read(output, 0, output.Length);
            return output;
        }

        public static Stream ToStream(this string input)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(input);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}