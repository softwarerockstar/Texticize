﻿//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;

namespace SoftwareRockstar.Texticize
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
        public static string ToStructuredText<T>(this IEnumerable<T> target, Func<T, string>[] columns, string colSeperator = "", string rowBeginDelimiter = "", string rowEndDelimiter = "")
        {
            StringBuilder sb = new StringBuilder();

            if (target != null)
                foreach (var t in target)
                {
                    sb.Append(rowBeginDelimiter);
                    sb.Append(columns.Select(w => w(t)).Aggregate((a, b) => a + colSeperator + b));
                    sb.Append(rowEndDelimiter);
                }

            return sb.ToString();
        }


        public static string ToStructuredText(this DataTable target, string columnNamesCsv, string colSeperator = "", string rowBeginDelimiter = "", string rowEndDelimiter = "")
        {
            StringBuilder sb = new StringBuilder();
            var columnNames = columnNamesCsv.Split(',');

            if (target != null)
                foreach (DataRow dr in target.Rows)
                {
                    List<string> oneRow = new List<string>();

                    foreach (var columnName in columnNames)
                        oneRow.Add(ToFormattedColumn(dr, columnName.Trim()));
                    
                    sb.Append(rowBeginDelimiter);
                    sb.Append(oneRow.Select(w => w).Aggregate((a, b) => a + colSeperator + b));
                    sb.Append(rowEndDelimiter);
                }

            return sb.ToString();
        }

        private static string ToFormattedColumn(DataRow row, string columnName)
        {
            var extendedProps = row.Table.Columns[columnName].ExtendedProperties;
            if (extendedProps.ContainsKey("Format"))
            {
                Delegate d = extendedProps["Format"] as Delegate;
                return d.DynamicInvoke(row[columnName]).ToString();                                    
            }                                    
            else
                return row[columnName].ToString();
        }


        public static string Lookup<T>(this IList<T> target, Func<T, bool> condition, Func<T, string> value)
        {
            return (value != null) ? value(target.Where(x => condition(x)).First()) : String.Empty;
        }

        public static Dictionary<string, string> ToParameterDictionary(this Capture capture, char parameterSeperatorChar)
        {
            Dictionary<string, string> parameterDictionary = new Dictionary<string, string>();

            if (capture != null)
            {
                var parameters = capture.Value.Split(parameterSeperatorChar).Select(s => s.Trim());

                foreach (string parameter in parameters)
                {
                    var paramParts = parameter.Split('=');

                    if (paramParts.Length > 1)
                        parameterDictionary.Add(paramParts[0].Trim(), paramParts[1].Trim());
                }
            }
            return parameterDictionary;
        }

        public static KeyValuePair<string, Delegate> MapTo<T>(this string pattern, Func<Context<T>, string> mapsTo)
        {
            return new KeyValuePair<string, Delegate>(pattern, mapsTo);
        }

        public static KeyValuePair<string, Delegate> MapTo(this string pattern, Func<Context<object>, string> mapsTo)
        {
            return new KeyValuePair<string, Delegate>(pattern, mapsTo);
        }

        public static KeyValuePair<string, object> ToVariable(this string variableName, object variable)
        {
            return new KeyValuePair<string, object>(variableName, variable);
        }

    }
}
