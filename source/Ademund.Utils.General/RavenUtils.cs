using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ademund.Utils
{
    public static class RavenUtils
    {
        private static readonly Regex escapeCharsRegex = new Regex(@"[\\+\-!():\^\[\]{}~?]");
        private static readonly Regex parseQueryReqex = new Regex(@"((?:(?:(?:AND|OR)\s)?(?:NOT\s)?))([^\s]+?):((?:(?<!\\)""[^""]+(?<!\\)"")|(?:[^\s]+))", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        public static string EscapeQueryValue(string queryValue)
        {
            return escapeCharsRegex.Replace(queryValue, @"\$0");
        }

        public static string GenerateQueryString(Dictionary<string, string> filterTerms)
        {
            string whereStr = string.Empty;
            foreach (KeyValuePair<string, string> term in filterTerms)
            {
                if ((term.Value != null) && (term.Value.Trim() != string.Empty))
                {
                    if (whereStr.Length > 0)
                        whereStr += " AND ";

                    string termKey = EscapeQueryValue(term.Key);
                    string termValue = term.Value;
                    if (termValue.StartsWith("NOT "))
                    {
                        termKey = string.Format("NOT {0}", termKey);
                        termValue = termValue.Substring(4);
                    }

                    if ((termValue.StartsWith("{") && termValue.EndsWith("}") && termValue.Contains(" TO ")) ||
                        (termValue.StartsWith("[") && termValue.EndsWith("]") && termValue.Contains(" TO ")) ||
                        (termValue.StartsWith("(") && termValue.EndsWith(")") && termValue.Contains(" OR ")))
                    {
                        whereStr += string.Format("{0}:{1}", termKey, termValue);
                    }
                    else
                    {
                        termValue = EscapeQueryValue(termValue);
                        if (termValue.Contains(" "))
                            whereStr += string.Format("{0}:\"{1}\"", termKey, termValue);
                        else
                            whereStr += string.Format("{0}:{1}", termKey, termValue);
                    }
                }
            }
            return whereStr;
        }

        public static Dictionary<string, string> ParseQueryString(string queryString)
        {
            var terms = new Dictionary<string, string>();

            Match matchResults = parseQueryReqex.Match(queryString);
            while (matchResults.Success)
            {
                string op = matchResults.Groups[1].Value;
                string key = matchResults.Groups[2].Value;
                string value = matchResults.Groups[3].Value;
                string filterValue = string.Empty;
                if (key.StartsWith("("))
                    key = key.Substring(1);

                // unescape the value
                value = Regex.Replace(value, @"(?:\\([\\+\-!():\^\]{}~?]))", "$1", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Multiline);

                if (op.StartsWith("AND "))
                    op = op.Substring("AND ".Length);
                if (value.EndsWith(")"))
                    value = value.Substring(0, value.Length - 1);

                filterValue = string.Format("{0}{1}", op, value);
                if (terms.ContainsKey(key) && !string.IsNullOrWhiteSpace(terms[key]))
                    filterValue = string.Format("{0} {1}", terms[key], filterValue);
                terms[key] = filterValue;

                matchResults = matchResults.NextMatch();
            }

            return terms;
        }
    }
}