using System.IO;
using System.IO.Compression;

namespace DotnetEkb.EfTesting.Tests.Helpers.CompressHelper
{
    public static class CompressHelper
    {
        private const int BufSize = 1024;

        public static MemoryStream Decompress(MemoryStream inp)
        {
            var outp = new MemoryStream();
            var buf = new byte[BufSize];
            using (var deflateStream = new DeflateStream(inp, CompressionMode.Decompress))
            {
                int len;
                while ((len = deflateStream.Read(buf, 0, buf.Length)) > 0)
                {
                    outp.Write(buf, 0, len);
                }
            }
            outp.Position = 0;
            return outp;
        }
    }
}
