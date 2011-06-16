//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses///      
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
        public static string ToDelimitedText<T>(this IList<T> target, Func<T, string>[] columns, string colBeginDelimiter = "", string colEndDelimiter = "", string rowBeginDelimiter = "", string rowEndDelimiter = "")
        {
            StringBuilder sb = new StringBuilder();
            foreach (var t in target)
            {
                sb.Append(rowBeginDelimiter);
                sb.Append(columns.Select(w => w(t)).Aggregate((a, b) => colBeginDelimiter + a + colEndDelimiter + colBeginDelimiter + b + colEndDelimiter));
                sb.Append(rowEndDelimiter);
            }
            return sb.ToString();
        }


        public static string Lookup<T>(this IList<T> target, Func<T, bool> condition, Func<T, string> value)
        {
            return value(target.Where(x => condition(x)).First());
        }

        public static Dictionary<string, string> ToParameterDictionary(this Group group)
        {
            Dictionary<string, string> parameterDictionary = new Dictionary<string, string>();

            var parameters = group.Value.Split(',').Select(s => s.Trim());

            foreach (string parameter in parameters)
            {
                var paramParts = parameter.Split('=');

                if (paramParts.Length > 1)
                    parameterDictionary.Add(paramParts[0], paramParts[1]);
            }

            return parameterDictionary;
        }
    }
}
