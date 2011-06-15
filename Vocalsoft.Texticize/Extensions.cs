using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vocalsoft.Texticize
{
    public static class Extensions
    {
        public static string ToFormattedTable<T>(this IList<T> target, Func<T, string>[] columns, string columnBegin="", string columnEnd="", string rowBegin="", string rowEnd="")
        {
            StringBuilder sb = new StringBuilder();

            foreach (var t in target)
            {
                sb.Append(rowBegin);

                foreach (var c in columns) 
                    sb.AppendFormat("{0}{1}{2}", columnBegin, c(t), columnEnd);

                sb.Append(rowEnd);
            }

            return sb.ToString();
        }


    }
}
