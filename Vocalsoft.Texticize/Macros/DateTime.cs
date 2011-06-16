using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace Vocalsoft.Texticize.Macros
{
    [Export(typeof(SystemMacro))]
    [ExportMetadata("Macro", "DateTime")]
    class DateTime : SystemMacro
    {
        public string GetValue(string macro)
        {
            string format = "M/d/yyyy";
            
            var parameters = MacroHelper.ParseParameters(macro, ' ');

            if (parameters.Count > 0)
                format = parameters[0];

            return System.DateTime.Now.ToString(format);
        }
    }
}
