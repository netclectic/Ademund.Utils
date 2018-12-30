using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ademund.Utils
{
    public static class PathUtils
	{
        public static string InvalidPathChars { get; } = new string(Path.GetInvalidPathChars());
        public static string InvalidFilenameChars { get; } = new string(Path.GetInvalidFileNameChars());

        public static string BadCharsInPath(string pathToCheck)
		{
			return string.Join(" ", pathToCheck.Where(x => InvalidPathChars.Contains(x)).Distinct().ToArray());
		}

		public static string BadCharsInFilename(string nameToCheck)
		{
			return string.Join(" ", nameToCheck.Where(x => InvalidFilenameChars.Contains(x)).Distinct().ToArray());
		}

		public static bool HasInvalidPathChars(string pathToCheck)
		{
			string invalidChars = System.Text.RegularExpressions.Regex.Escape(InvalidPathChars);
			string invalidReStr = string.Format("([:{0}]+)", invalidChars);
			return System.Text.RegularExpressions.Regex.IsMatch(pathToCheck, invalidReStr);
		}

		public static bool HasInvalidFilenameChars(string nameToCheck)
		{
			string invalidChars = System.Text.RegularExpressions.Regex.Escape(InvalidFilenameChars);
			string invalidReStr = string.Format("([{0}]+)", invalidChars);
			return System.Text.RegularExpressions.Regex.IsMatch(nameToCheck, invalidReStr);
		}

		public static string SanitizeFilePath(string filePath, string replaceChar="_")
		{
			filePath = filePath.Replace("/", Path.DirectorySeparatorChar.ToString());
            filePath = filePath.Replace(@"\\", Path.DirectorySeparatorChar.ToString());
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(InvalidPathChars);
			string invalidReStr = string.Format( "([:{0}]+)", invalidChars );
			return System.Text.RegularExpressions.Regex.Replace( filePath, invalidReStr, replaceChar );
		}

		public static string SanitizeFileName(string fileName, string replaceChar="_")
		{
			string invalidChars = System.Text.RegularExpressions.Regex.Escape(InvalidFilenameChars);
			string invalidReStr = string.Format( "([{0}]+)", invalidChars );
			return System.Text.RegularExpressions.Regex.Replace(fileName, invalidReStr, replaceChar);
		}

		public static string SanitizeFullFilePath(string filePath, string replaceChar="_")
		{
			filePath = SanitizeFilePath(filePath, replaceChar);

            string pathRoot = Path.GetPathRoot(filePath);
            filePath = filePath.Substring(pathRoot.Length);
            string dir = Path.GetDirectoryName(filePath);
            string filename = SanitizeFileName(Path.GetFileName(filePath), replaceChar);
            return Path.Combine(pathRoot, dir, filename);
		}

		public static string GetLocalFilenameFromUri(Uri uri)
		{
			string scheme = PathUtils.SanitizeFilePath(uri.Scheme).Trim(Path.DirectorySeparatorChar);
			string host = PathUtils.SanitizeFilePath(uri.Host).Trim(Path.DirectorySeparatorChar);
			string path = PathUtils.SanitizeFilePath(uri.AbsolutePath).Trim(Path.DirectorySeparatorChar);
			string filename = Path.GetFileNameWithoutExtension(path);
			string query = uri.Query;
			string ext = Path.GetExtension(uri.AbsolutePath);

			if (string.IsNullOrWhiteSpace(filename))
				filename = Guid.NewGuid().ToString();
			if (string.IsNullOrWhiteSpace(ext))
				ext = ".html";
			if (!string.IsNullOrWhiteSpace(query))
				filename = PathUtils.SanitizeFileName(filename + query);
			if (!string.IsNullOrWhiteSpace(path))
				path = Path.GetDirectoryName(path);
			filename += ext;

            host = host.Replace('.', '_');
            path = path.Replace('.', '_');

            string filePath = Path.Combine(new[] { scheme, host, path, filename });
			filePath = PathUtils.SanitizeFullFilePath(filePath);

			return System.Net.WebUtility.UrlDecode(filePath);
		}

        public static string MakePathRelativeTo(string basePath, string fullPath)
        {
			string[] absDirs = basePath.Split(Path.DirectorySeparatorChar);
			string[] relDirs = fullPath.Split(Path.DirectorySeparatorChar);
           // Get the shortest of the two paths 
            int len = absDirs.Length < relDirs.Length ? absDirs.Length : relDirs.Length;
            // Use to determine where in the loop we exited 
            int lastCommonRoot = -1; int index;
            // Find common root 
            for (index = 0; index < len; index++)
            {
                if (absDirs[index] == relDirs[index])
                    lastCommonRoot = index;
                else
                    break;
            }
            // If we didn't find a common prefix then throw 
            if (lastCommonRoot == -1)
            {
                throw new ArgumentException("Paths do not have a common base");
            }
            // Build up the relative path 
            var relativePath = new StringBuilder();
            // Add on the .. 
            for (index = lastCommonRoot + 1; index < absDirs.Length; index++)
            {
                if (absDirs[index].Length > 0)
                    relativePath.AppendFormat("..{0}", Path.DirectorySeparatorChar);
            }
            // Add on the folders 
            for (index = lastCommonRoot + 1; index < relDirs.Length - 1; index++)
            {
                relativePath.Append(relDirs[index]).Append(Path.DirectorySeparatorChar.ToString());
            }
            relativePath.Append(relDirs[relDirs.Length - 1]);
            return relativePath.ToString();
        }

        public static Tuple<string, bool> GetRelativeRootPath(string basePath, string fullPath)
        {
            string rel = MakePathRelativeTo(basePath, fullPath);
            string[] parts = rel.Split(Path.DirectorySeparatorChar);
            return Tuple.Create(parts[0], parts.Length == 1);
        }

        public static Tuple<bool, string> IsRelativeRootPath(string basePath, string fullPath)
        {
            string rel = MakePathRelativeTo(basePath, fullPath);
            string[] parts = rel.Split(Path.DirectorySeparatorChar);
            return Tuple.Create(parts.Length == 1, parts[0]);
        }

        public static IEnumerable<string> EnumerateFilesFilter(string path, string filesFilter, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return filesFilter.Split(',', ';', '|').SelectMany(_ => Directory.EnumerateFiles(path, "*" + _, searchOption));
        }
    }
}