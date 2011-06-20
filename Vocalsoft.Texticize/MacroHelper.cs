using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vocalsoft.Texticize
{
    static class MacroHelper
    {
        public static List<string> ParseParameters(string macro, char seperator)
        {
            var toReturn = new List<string>(macro.Split(seperator));
            if (toReturn.Count > 0)
            {
                toReturn.RemoveAt(0);
                toReturn.RemoveAll(s => s.Trim() == String.Empty);
            }

            return toReturn;
        }
    }
}
