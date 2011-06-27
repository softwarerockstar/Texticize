//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System.ComponentModel.Composition;
using Vocalsoft.ComponentModel;

namespace Vocalsoft.Texticize.MacroProcessors
{
    [Export(typeof(IMacroProcessor))]
    [ExportMetadata(UniquenessEvidenceFields.UniqueName, SystemMacroProcessorNames.UserDomainName)]
    class UserDomainNameMacroProcessor : IMacroProcessor
    {
        public string GetValue(string macro)
        {
            return System.Environment.UserDomainName;
        }
    }
}
