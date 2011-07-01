//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System.ComponentModel.Composition;
using SoftwareRockstar.ComponentModel.Extensibility;
using System.Collections.Generic;
using System;

namespace SoftwareRockstar.Texticize.MacroProcessors
{
    [Export(typeof(IMacroProcessor))]
    [ExportMetadata(UniquenessEvidenceFields.UniqueName, SystemMacroProcessorNames.Echo)]
    class EchoMacroProcessor : IMacroProcessor
    {
        public string GetValue(string macro)
        {
            string toReturn = String.Empty;

            if (!String.IsNullOrEmpty(macro))
            {
                var indexOfWhiteSpace = macro.IndexOfAny(new char[] { ' ', '\t' });
                if (indexOfWhiteSpace > 0 && macro.Length > (indexOfWhiteSpace + 1))
                    toReturn = macro.Substring(indexOfWhiteSpace + 1);
            }

            return toReturn;
        }
    }
}
