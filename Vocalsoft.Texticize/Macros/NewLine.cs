using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace Vocalsoft.Texticize.Macros
{
    [Export(typeof(SystemMacro))]
    [ExportMetadata("Macro", "NewLine")]
    class NewLine : SystemMacro
    {
        public string GetValue(string macro)
        {
            return System.Environment.NewLine;
        }
    }
}
