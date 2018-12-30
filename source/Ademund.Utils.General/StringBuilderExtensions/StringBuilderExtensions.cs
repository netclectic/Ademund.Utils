using System.Text;

namespace Ademund.Utils.StringBuilderExtensions
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendLineFormat(this StringBuilder builder, string format, params object[] arguments)
        {
            string value = string.Format(format, arguments);
            builder.AppendLine(value);
            return builder;
        }
    }
}