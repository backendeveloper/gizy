using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using static System.String;

namespace Gizy.Extensions
{
    public static class StringExtensions
    {
        private static Dictionary<string, string> _htmlReplacementCharacters;

        public static T ToEnum<T>(this string value)
        {
            if (IsNullOrEmpty(value))
            {
                return (T)Enum.Parse(typeof(T), "Undefined", true);
            }
    
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string ToStringOrEmpty(this object value)
        {
            return value == null ? Empty : value.ToString();
        }

        public static List<T> ToList<T>(this string value, string[] delimiters = null)
        {
            if (IsNullOrEmpty(value))
            {
                return null;
            }
    
            if (delimiters == null || delimiters.Length == 0)
            {
                delimiters = new[] { "," };
            }
    
            string[] splitArray = value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
    
            if (splitArray == null || splitArray.Length == 0)
            {
                return null;
            }
    
            return splitArray.Select(text => (T) Convert.ChangeType(text, typeof(T))).ToList();
        }

        public static List<int> ToSplitIntegerList(this string value, string[] delimiters = null)
        {
            if (IsNullOrEmpty(value))
            {
                return null;
            }
    
            if (delimiters == null || delimiters.Length == 0)
            {
                delimiters = new[] { "," };
            }
    
            string[] splitArray = value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
    
            if (splitArray == null || splitArray.Length == 0)
            {
                return null;
            }
    
            List<int> resultList = new List<int>();
    
            foreach (var t in splitArray)
            {
                if (int.TryParse(t, out var temp))
                {
                    resultList.Add(temp);
                }
            }
    
            return resultList;
        }

        public static List<string> ToSplitStringList(this string value, string[] delimiters = null)
        {
            if (IsNullOrEmpty(value))
            {
                return null;
            }
    
            if (delimiters == null || delimiters.Length == 0)
            {
                delimiters = new[] { "," };
            }
    
            string[] splitArray = value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
    
            if (splitArray == null || splitArray.Length == 0)
            {
                return null;
            }
    
            return splitArray.ToList();
        }

        public static bool ToBool(this string value)
        {
            if (IsNullOrEmpty(value))
            {
                return false;
            }
    
            switch (value.ToLower())
            {
                case "true":
                    return true;
                case "a":
                    return true;
                case "y":
                    return true;
                case "1":
                    return true;
            }
    
            return false;
        }

        public static long ToLong(this string value, long defaultValue = 0)
        {
            return long.TryParse(value, out var result) ? result : defaultValue;
        }

        public static int ToInt(this string value, int defaultValue = 0)
        {
            return int.TryParse(value, out var result) ? result : defaultValue;
        }

        public static string Truncate(this string value, int length = 25)
        {
            if (IsNullOrEmpty(value))
            {
                return Empty;
            }
    
            return value.Length <= length ? value : Join("", value.Substring(0, length - 3), "...");
        }

        public static string ToSlug(this string title, List<string> ignoredCharacters = null)
        {
            if (IsNullOrEmpty(title))
            {
                return Empty;
            }
    
            if (ignoredCharacters == null)
            {
                ignoredCharacters = new List<string>();
            }
    
            if (_htmlReplacementCharacters == null)
            {
                _htmlReplacementCharacters = GenerateHtmlReplacementCharacters();
            }
    
            title = title.ToLower().Trim();
            title = Regex.Replace(title, "<.*?>", Empty);

            return _htmlReplacementCharacters.Where(pair => !ignoredCharacters.Contains(pair.Key)).Aggregate(title, (current, pair) => current.Replace(pair.Key, pair.Value));
        }

        public static string ToTitleCase(this string input)
        {
            return ToTitleCase(input, new CultureInfo("tr-TR"));
        }

        public static string ToTitleCase(this string input, CultureInfo cultureInfo)
        {
            TextInfo textInfo = cultureInfo.TextInfo;
            
            return textInfo.ToTitleCase(input.ToLower());
        }
        
        private static Dictionary<string, string> GenerateHtmlReplacementCharacters()
        {
            Dictionary<string, string> results = new Dictionary<string, string>
            {
                {"ı", "i"},
                {"ş", "s"},
                {"ğ", "g"},
                {"ü", "u"},
                {"ç", "c"},
                {"ö", "o"},
                {" ", "-"},
                {"?", Empty},
                {":", Empty},
                {"/", Empty},
                {"%", Empty},
                {".", "-"},
                {"+", "-"},
                {"\\", "-"},
                {"&", Empty},
                {"'", Empty},
                {char.ConvertFromUtf32(34), Empty},
                {"#", Empty},
                {"--", "-"},
                {"*", Empty},
                {"<", Empty},
                {">", Empty}
            };
    
            return results;
        }
    }
}