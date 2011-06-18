using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Vocalsoft.ComponentModel;
using System.ComponentModel.Composition;

namespace Vocalsoft.Texticize.Processors
{
    [Export(typeof(IProcessor))]
    [ExportMetadata("Processor", "Macro")]
    class MacroProcessor : IProcessor
    {
        /// <summary>
        /// Processes macros and performs substitutions
        /// </summary>
        public ProcessorOutput Process(ProcessorInput input)
        {
            ProcessorOutput output = new ProcessorOutput();
            output.Result = input.Target;

            try
            {
                Regex regex = new Regex(input.Configuration.MacroRegexPatternFormatted, input.Configuration.MacroRegexOptions);
                var plugins = ExtensibilityHelper<ISystemMacro, ISystemMacroMetaData>.Current;

                var matches = regex.Matches(output.Result);

                for (int i = 0; i < matches.Count; i++)
                {
                    var match = matches[i];

                    if (match.Success)
                    {
                        string macro = match.Value.Substring(1, match.Value.Length - 2);
                        var processor = plugins.GetPlugins(s => macro.StartsWith(s.Metadata.Macro)).FirstOrDefault();
                        output.Result = output.Result.Replace(match.Value, processor.GetValue(macro));
                    }
                }
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Exceptions.Add(ex);
            }

            return output;
        }
    }
}
