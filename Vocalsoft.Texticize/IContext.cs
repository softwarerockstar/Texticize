using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vocalsoft.Texticize
{
    interface IContext
    {
        void SetProperties(object variable, string variableName, string expression, IList<string> regexGroups);
    }
}
