using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Vocalsoft.Texticize;

namespace Vocalsoft.Texticize.UnitTests
{
    [Export(typeof(IProcessor))]
    [ExportMetadata("Processor", "Test")]
    class TestProcessor : IProcessor
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
