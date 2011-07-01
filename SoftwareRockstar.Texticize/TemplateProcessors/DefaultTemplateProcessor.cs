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
            ProcessorOutput output = new ProcessorOutput { Result = base.ProcessInput.Target };

            try
            {
                foreach (var processorName in base.ProcessInput.Configuration.ProcessorPipeline)
                {
                    var processor = SubstitutionProcessorFactory.Create(processorName);
                    if (processor != null)
                    {
                        try
                        {
                            output.Combine(processor.Process(base.ProcessInput));
                        }
                        catch (ApplicationException pex)
                        {
                            output.Exceptions.Add(pex);
                        }

                        if (!output.IsSuccess)
                            if (!base.ProcessInput.Configuration.ContinueOnError)
                                break;

                        base.ProcessInput.Target = output.Result;
                    }
                }
            }
            catch (ApplicationException ex)
            {
                output.Exceptions.Add(ex);
            }

            return output;
        }

    }
}
