using System.IO;

namespace DotnetEkb.EfTesting.Tests.Helpers.FilesAndDirectoryHelpers
{
    public static class FileHelper
    {
        public static byte[] FileToByteArray(string filePath)
        {
            using (var fs = File.OpenRead(filePath))
            {
                using (var ms = new MemoryStream())
                {
                    fs.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}