using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace APIDog.Extensions
{
    public static class StringExtensions
    {
        public static string FirstToUpper(this string line)
        {
            if (line == null)
                return "null";
            var ch = char.ToUpper(line[0]);
            return ch.ToString() + line.Substring(1, line.Length - 1);
        }

        public static string QueryName(this string url)
        {
            return Regex.Match(url, @"(\w*:(//[^/?#]+)?/)?(?<folder>[^?#]+)").Groups["folder"].Value;
        }

        public static IEnumerable<string> QueryParams(this string url)
        {
            var split = url.Split('?');
            if (split.Length != 2)
                yield break;

            foreach (var item in split[1].Split('&'))
            {
                yield return item.Split('=')[0];
            }
            yield break;
        }

        public static string WithoutParams(this string url)
        {
            return url.Split('?')[0];
        }

        public static string WithoutUrl(this string url)
        {
            return url.Split('?')[1];
        }

        public static int WithoutUrlCount(this string url)
        {
            return url.Split('?').Count();
        }
    }
}