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

namespace Vocalsoft.Texticize.UnitTests
{
    [Export(typeof(ISubstitutionProcessor))]
    [ExportMetadata(UniquenessEvidenceFields.UniqueName, "Test")]
    class TestProcessor : ISubstitutionProcessor
    {
        /// <summary>
        /// Processes macros and performs substitutions
        /// </summary>
        public ProcessorOutput Process(ProcessorInput input)
        {
            ProcessorOutput output = new ProcessorOutput();
            output.Result = "I am tested: " + input.Target;
            
            return output;
        }
    }
}
