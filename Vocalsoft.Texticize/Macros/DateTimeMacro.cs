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
    [ExportMetadata("Macro", SystemMacros.DateTime)]
    class DateTimeMacro : ISystemMacro
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
