using System;
using System.IO;
using System.Text;

namespace Ademund.Utils.StringExtensions
{
    public static class StringExtensions
    {
        public static Stream ToStream(this string str)
        {
            var stream = new MemoryStream();
            using (var writer = new StreamWriter(stream, Encoding.UTF8, 4096, leaveOpen: true))
            {
                writer.Write(str);
                writer.Flush();
            }
            stream.Position = 0;
            return stream;
        }

        public static Type GetTypeFromSimpleName(this string typeName)
        {
            if (typeName == null)
                throw new ArgumentNullException(nameof(typeName));

            bool isArray = false, isNullable = false;

            if (typeName.IndexOf("[]") != -1)
            {
                isArray = true;
                typeName = typeName.Remove(typeName.IndexOf("[]"), 2);
            }

            if (typeName.IndexOf("?") != -1)
            {
                isNullable = true;
                typeName = typeName.Remove(typeName.IndexOf("?"), 1);
            }

            typeName = typeName.ToLower();

            string parsedTypeName = null;
            switch (typeName)
            {
                case "bool":
                case "boolean":
                    parsedTypeName = "System.Boolean";
                    break;
                case "byte":
                    parsedTypeName = "System.Byte";
                    break;
                case "char":
                    parsedTypeName = "System.Char";
                    break;
                case "datetime":
                    parsedTypeName = "System.DateTime";
                    break;
                case "datetimeoffset":
                    parsedTypeName = "System.DateTimeOffset";
                    break;
                case "decimal":
                    parsedTypeName = "System.Decimal";
                    break;
                case "double":
                    parsedTypeName = "System.Double";
                    break;
                case "float":
                    parsedTypeName = "System.Single";
                    break;
                case "int16":
                case "short":
                    parsedTypeName = "System.Int16";
                    break;
                case "int32":
                case "int":
                    parsedTypeName = "System.Int32";
                    break;
                case "int64":
                case "long":
                    parsedTypeName = "System.Int64";
                    break;
                case "object":
                    parsedTypeName = "System.Object";
                    break;
                case "sbyte":
                    parsedTypeName = "System.SByte";
                    break;
                case "string":
                    parsedTypeName = "System.String";
                    break;
                case "timespan":
                    parsedTypeName = "System.TimeSpan";
                    break;
                case "uint16":
                case "ushort":
                    parsedTypeName = "System.UInt16";
                    break;
                case "uint32":
                case "uint":
                    parsedTypeName = "System.UInt32";
                    break;
                case "uint64":
                case "ulong":
                    parsedTypeName = "System.UInt64";
                    break;
            }

            if (parsedTypeName != null)
            {
                if (isArray)
                    parsedTypeName += "[]";

                if (isNullable)
                    parsedTypeName = string.Concat("System.Nullable`1[", parsedTypeName, "]");
            }
            else
            {
                parsedTypeName = typeName;
            }

            // Expected to throw an exception in case the type has not been recognized.
            return Type.GetType(parsedTypeName);
        }

        public static string EncodeToBase64(this string str)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string DecodeFromBase64(this string str)
        {
            byte[] decodedBytes = Convert.FromBase64String(str);
            return Encoding.UTF8.GetString(decodedBytes);
        }

        public static string Replace(this string str, string oldValue, string newValue, StringComparison comparison)
        {
            var sb = new StringBuilder();

            int previousIndex = 0;
            int index = str.IndexOf(oldValue, comparison);
            while (index != -1)
            {
                sb.Append(str, previousIndex, index - previousIndex);
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }
            sb.Append(str.Substring(previousIndex));

            return sb.ToString();
        }

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static string UppercaseFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static string ReplaceFirst(this string text, string search, string replace, StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase)
        {
            int pos = text.IndexOf(search, stringComparison);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }
}
