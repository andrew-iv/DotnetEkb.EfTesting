using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotnetEkb.EfTesting.Tests.Helpers.FilesAndDirectoryHelpers
{
    public static class FileNameToFileTypeIcon
    {
        //private static string IconsPath
        //{
        //    get { return new UrlHelper(HttpContext.Current.Request.RequestContext).Content("~/Content/img/file_icons/"); }
        //}

        static FileNameToFileTypeIcon()
        {
            FileExtensions.Add("bmp", "bmp");
            FileExtensions.Add("doc", "doc");
            FileExtensions.Add("docx", "doc");
            FileExtensions.Add("jpg", "jpg");
            FileExtensions.Add("jpeg", "jpg");
            FileExtensions.Add("pdf", "pdf");
            FileExtensions.Add("png", "png");
            FileExtensions.Add("ppt", "ppt");
            FileExtensions.Add("pptx", "ppt");
            FileExtensions.Add("rar", "rar");
            FileExtensions.Add("txt", "txt");
            FileExtensions.Add("xls", "xls");
            FileExtensions.Add("xlsx", "xls");
            FileExtensions.Add("zip", "zip");
        }

        public static string GetFileWithIcon(string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            fileExtension = String.IsNullOrEmpty(fileExtension) ? "" : fileExtension.Substring(1).ToLower();
            //var stringBuilder = new StringBuilder(IconsPath);
            var stringBuilder = new StringBuilder("IconsPath");
            if (FileExtensions.ContainsKey(fileExtension))
            {
                stringBuilder.Append(FileExtensions[fileExtension]).Append(".png");
            }
            else
            {
                stringBuilder.Append(DefaultFile);
            }
            return stringBuilder.ToString();
        }

        public static string GetFileWithIconForBasePath(string fileName, string path)
        {
            var fileExtension = Path.GetExtension(fileName);
            fileExtension = String.IsNullOrEmpty(fileExtension) ? "" : fileExtension.Substring(1).ToLower();
            var stringBuilder = new StringBuilder(path);
            if (FileExtensions.ContainsKey(fileExtension))
            {
                stringBuilder.Append(FileExtensions[fileExtension]).Append(".png");
            }
            else
            {
                stringBuilder.Append(DefaultFile);
            }
            return stringBuilder.ToString();
        }

        private static readonly Dictionary<string, string> FileExtensions = new Dictionary<string, string>();
        private const string DefaultFile = "_blank.png";
    }
}