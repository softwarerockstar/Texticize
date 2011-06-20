using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.IO;

namespace Vocalsoft.Texticize.Macros
{
    [Export(typeof(ISystemMacro))]
    [ExportMetadata("Macro", SystemMacros.Include)]
    class IncludeMacro : ISystemMacro
    {
        public string GetValue(string macro)
        {
            string filePath = MacroHelper.ParseParameters(macro, ' ')[0];
            return File.ReadAllText(filePath);
        }
    }
}
