using System.IO;

namespace DotnetEkb.EfTesting.Tests.Helpers.FilesAndDirectoryHelpers
{
    public static class CopyFolderHelper
    {
        public static void CopyFolderWithEntire(string sourceFolder, string destinationFolder)
        {
            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);
            foreach (string dirPath in Directory.GetDirectories(sourceFolder, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourceFolder, destinationFolder));
            foreach (string newPath in Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(sourceFolder, destinationFolder), true);
        }
    }
}