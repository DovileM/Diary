using System;
using System.Text.RegularExpressions;

namespace Diary
{
    public static class StringExtension
    {
        public static string TrimAndReduce(this string str)
        {
            return ConvertWhitespacesToSingleSpaces(str).Trim();
        }

        public static string ConvertWhitespacesToSingleSpaces(this string value)
        {
            return Regex.Replace(value, @"\s+", " ");
        }

        public static string ConvertFirst(this string str)
        {
            str = char.ToUpper(str[0]) + str.Substring(1, str.Length - 1);
            return str;
        }
        public static string ConvertSecond(this string str)
        {
            var s = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            str = s[0] + " " + char.ToUpper(s[1][0]) + s[1].Substring(1, s[1].Length - 1);
            return str;
        }
    }

}
