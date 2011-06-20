using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace Vocalsoft.Texticize.Macros
{
    [Export(typeof(ISystemMacro))]
    [ExportMetadata("Macro", SystemMacros.UserName)]
    class UserNameMacro : ISystemMacro
    {
        public string GetValue(string macro)
        {
            return System.Environment.UserName;
        }
    }
}
