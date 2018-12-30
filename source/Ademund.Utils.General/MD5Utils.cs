using System.IO;

namespace Ademund.Utils
{
    public static class MD5Utils
    {
        public static string GenerateMD5Hash(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] data = md5.ComputeHash(stream);
                return ConvertMD5HashToString(data);
            }
        }

        public static string GenerateMD5Hash(string str)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(str);
                byte[] data = md5.ComputeHash(inputBytes);

                return ConvertMD5HashToString(data);
            }
        }

        private static string ConvertMD5HashToString(byte[] data)
        {
            var sBuilder = new System.Text.StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }
    }
}