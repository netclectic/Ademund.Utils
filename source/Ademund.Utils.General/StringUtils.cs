using System.Text;
using System.Text.RegularExpressions;

namespace Ademund.Utils
{
    public static class StringUtils
	{
		private static string MaximumAllowedCharacterReplacement(Match m)
		{
			// You can vary the replacement text for each match on-the-fly
			var result = new StringBuilder();
			foreach (char ch in m.Value)
			{
                result.AppendFormat("&#{0};", (int)ch);
			}
			return result.ToString();
		}

		public static string EntityEncodeString(string stringToEncode, int maxAllowedCharacter = 126)
		{
			if (maxAllowedCharacter == 0)
				return stringToEncode;

			string hexMaximumAllowedCharacter = maxAllowedCharacter.ToString("X");
            stringToEncode = Regex.Replace(stringToEncode, "[\\x00-\\x1F]", "");
            return Regex.Replace(stringToEncode, string.Format("[^\\x20-\\x{0}]", hexMaximumAllowedCharacter), new MatchEvaluator(MaximumAllowedCharacterReplacement), RegexOptions.Singleline | RegexOptions.Multiline);
		}

		public static string ConvertStringToUtf8(string inputString, string currentEncoding, int MaximumAllowedCharacter = 0, bool forceMaximumAllowedCharacter = false)
		{
			// if it's already utf8 then just return it
			var encoding = Encoding.GetEncoding(currentEncoding);
			if ((encoding == Encoding.UTF8) && !forceMaximumAllowedCharacter)
				return inputString;

			// if it's not utf8 then convert it to utf8 and entity encode any characters > 255
			byte[] encBytes = encoding.GetBytes(inputString);
			byte[] utf8Bytes = Encoding.Convert(encoding, Encoding.UTF8, encBytes);
			string utf8String = Encoding.UTF8.GetString(utf8Bytes);

			return EntityEncodeString(utf8String, MaximumAllowedCharacter);
		}
	}
}
