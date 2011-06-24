//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System.ComponentModel.Composition;

namespace Vocalsoft.Texticize.Macros
{
    [Export(typeof(ISystemMacro))]
    [ExportMetadata("Macro", SystemMacros.UserDomainName)]
    class UserDomainNameMacro : ISystemMacro
    {
        public string GetValue(string macro)
        {
            return System.Environment.UserDomainName;
        }
    }
}
