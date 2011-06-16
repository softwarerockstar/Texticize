using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace Vocalsoft.Texticize.Macros
{
    [Export(typeof(SystemMacro))]
    [ExportMetadata("Macro", "UserDomainName")]
    class UserDomainName : SystemMacro
    {
        public string GetValue(string macro)
        {
            return System.Environment.UserDomainName;
        }
    }
}
