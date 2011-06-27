//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System.ComponentModel.Composition;
using Vocalsoft.ComponentModel.Extensibility;

namespace Vocalsoft.Texticize.MacroProcessors
{
    [Export(typeof(IMacroProcessor))]
    [ExportMetadata(UniquenessEvidenceFields.UniqueName, SystemMacroProcessorNames.DateTime)]
    class DateTimeMacroProcessor : IMacroProcessor
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
