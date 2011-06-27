//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace SoftwareRockstar.Texticize
{
    static class MacroHelper
    {
        public static List<string> ParseParameters(string macro, char seperator)
        {
            var toReturn = new List<string>(macro.Split(seperator));
            if (toReturn.Count > 0)
            {
                toReturn.RemoveAt(0);
                toReturn.RemoveAll(s => String.IsNullOrEmpty(s.Trim()));
            }

            return toReturn;
        }
    }
}
