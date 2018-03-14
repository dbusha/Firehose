using System;

namespace Firehose.Utils
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhitespace(this string item)
        { return string.IsNullOrWhiteSpace(item); }


        public static bool IsEqualTo(this string first, string second)
        { return first.Equals(second, StringComparison.InvariantCultureIgnoreCase); }
        
        
        public static bool IsEqualToCs(this string first, string second)
        { return first.Equals(second); }
    }
}