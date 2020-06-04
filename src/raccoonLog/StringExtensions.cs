using System.Text;

namespace raccoonLog
{
    internal static class StringExtensions
    {
        public static StringBuilder AppendLine(this StringBuilder builder, string format, params object[] args)
        {
            builder.AppendLine().AppendFormat(format, args);
            return builder;
        }
    }
}