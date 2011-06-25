using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vocalsoft.Texticize.Factories;
using System.ComponentModel.Composition;
using Vocalsoft.Texticize.SubstitutionProcessors;

namespace Vocalsoft.Texticize
{   
    [Serializable]
    [Export(typeof(ITemplateProcessor))]
    [ExportMetadata("UniqueName", TemplateProcessorNames.Default)]
    public class DefaultTemplateProcessor : AbstractTemplateProcessor
    {
        public override ProcessorOutput Process()
        {
            var exceptions = new List<Exception>();

            ProcessorOutput currentProcessorOutput = new ProcessorOutput { Result = base.ProcessInput.Target };
            foreach (var processorName in base.ProcessInput.Configuration.ProcessorPipeline)
            {
                var processor = SubstitutionProcessorFactory.GetProcessor(processorName);
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
