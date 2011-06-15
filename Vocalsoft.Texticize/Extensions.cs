using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vocalsoft.Texticize
{
    public static class Extensions
    {
        /// <summary>
        /// Converts a List(T) to string representation like HTML table where each begin and end tag can be customized
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="columns"></param>
        /// <param name="columnBegin"></param>
        /// <param name="columnEnd"></param>
        /// <param name="rowBegin"></param>
        /// <param name="rowEnd"></param>
        /// <returns></returns>
        public static string ToFormattedTable<T>(this IList<T> target, Func<T, string>[] columns, string colBeginTag="", string colEndTag="", string rowBeginTag="", string rowEndTag="")
        {
            StringBuilder sb = new StringBuilder();
            foreach (var t in target)
            {
                sb.Append(rowBeginTag);
                sb.Append(columns.Select(w => w(t)).Aggregate((a, b) => colBeginTag + a + colEndTag + colBeginTag + b + colEndTag));
                sb.Append(rowEndTag);
            }
            return sb.ToString();
        }


        public static string Lookup<T>(this IList<T> target, Func<T, bool> condition, Func<T, string> value)
        {
            return value(target.Where(x => condition(x)).First());
        }

    }
}
