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

namespace SoftwareRockstar.Texticize.MockExtensions
{   
    [Serializable]
    [Export(typeof(ITemplateProcessor))]
    [ExportMetadata(UniquenessEvidenceFields.UniqueName, "SoftwareRockstar.Texticize.MockExtensions.CustomTemplateProcessorMock")]
    public class CustomTemplateProcessorMock : DefaultTemplateProcessor
    {
        public override ProcessorOutput Process()
        {
            var output = base.Process();
            output.Result = "<<Custom>>" + output.Result;
            return output;
        }

    }
}
