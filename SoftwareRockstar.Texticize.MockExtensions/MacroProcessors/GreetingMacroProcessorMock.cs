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
using System;

namespace SoftwareRockstar.Texticize.MockExtensions
{
    [Export(typeof(IMacroProcessor))]
    [ExportMetadata(UniquenessEvidenceFields.UniqueName, "Greeting")]
    class GreetingMacroProcessorMock : IMacroProcessor
    {
        public string GetValue(string macro)
        {   
            return "Hello: ";
        }
    }
}