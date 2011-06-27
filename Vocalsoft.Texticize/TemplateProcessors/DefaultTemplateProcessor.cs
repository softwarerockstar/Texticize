//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using SoftwareRockstar.ComponentModel.Extensibility;
using SoftwareRockstar.Texticize.Factories;

namespace SoftwareRockstar.Texticize
{   
    [Serializable]
    [Export(typeof(ITemplateProcessor))]
    [ExportMetadata(UniquenessEvidenceFields.UniqueName, SystemTemplateProcessorNames.Default)]
    public class DefaultTemplateProcessor : AbstractTemplateProcessor
    {
        public override ProcessorOutput Process()
        {
            var exceptions = new List<Exception>();

            ProcessorOutput currentProcessorOutput = new ProcessorOutput { Result = base.ProcessInput.Target };
            foreach (var processorName in base.ProcessInput.Configuration.ProcessorPipeline)
            {
                var processor = SubstitutionProcessorFactory.Create(processorName);
                if (processor != null)
                {
                    currentProcessorOutput = processor.Process(base.ProcessInput);

                    if (!currentProcessorOutput.IsSuccess)
                    {
                        exceptions.AddRange(currentProcessorOutput.Exceptions);

                        if (!base.ProcessInput.Configuration.ContinueOnError)
                            break;
                    }

                    base.ProcessInput.Target = currentProcessorOutput.Result;
                }
            }

            return new ProcessorOutput(
                currentProcessorOutput.Result,
                (exceptions.Count == 0),
                exceptions);
        }

    }
}
