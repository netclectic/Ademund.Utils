using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Ademund.Utils
{
	public static class UriUtils
	{
        /*
         This fixes a known bug in the Uri class, where dots are not allowed in the path, e.g. http://x/y./z
         This can be called from a pipeline task like so:

            clr.AddReference("Ademund.Utils")
            from Ademund.Utils import UriUtils

            UriUtils.ResetStaticFlagInUri()

         
            http://stackoverflow.com/questions/856885/httpwebrequest-to-url-with-dot-at-the-end
            https://social.msdn.microsoft.com/Forums/vstudio/en-US/5206beca-071f-485d-a2bd-657d635239c9/bug-in-uri-class-with-periods?forum=netfxbcl
        */
        public static void ResetStaticFlagInUri()
		{
			MethodInfo getSyntax = typeof(UriParser).GetMethod("GetSyntax", BindingFlags.Static | BindingFlags.NonPublic);
			FieldInfo flagsField = typeof(UriParser).GetField("m_Flags", BindingFlags.Instance | BindingFlags.NonPublic);
			if (getSyntax != null && flagsField != null)
			{
				foreach (string scheme in new[] { "http", "https" })
				{
					var parser = (UriParser)getSyntax.Invoke(null, new object[] { scheme });
					if (parser != null)
					{
						int flagsValue = (int)flagsField.GetValue(parser);
						// Clear the CanonicalizeAsFilePath attribute
						if ((flagsValue & 0x1000000) != 0)
							flagsField.SetValue(parser, flagsValue & ~0x1000000);
					}
				}
			}
		}

        private static readonly Lazy<Regex> regexResolver = new Lazy<Regex>(() => new Regex(@"[\r\n\t\a\f\v]", RegexOptions.Compiled), true);

        public static string FullyDecodeUrl(string url)
        {
            // remove any known non-printable characters
            url = regexResolver.Value.Replace(url, "");

            // fully decode the url
            while (url != System.Net.WebUtility.UrlDecode(url))
                url = System.Net.WebUtility.UrlDecode(url);
            return url;
        }

        public static string AbsolutizeRelativeLink(Uri baseUri, string relativeLink)
        {
            return new Uri(baseUri, relativeLink).ToString();
        }

        public static string RelativizeAbsoluteLink(Uri baseUri, string absoluteLink)
        {
            return baseUri.MakeRelativeUri(new Uri(absoluteLink)).ToString();
        }
    }
}
