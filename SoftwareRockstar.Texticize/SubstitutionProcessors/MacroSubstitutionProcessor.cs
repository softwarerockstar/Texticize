﻿//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using SoftwareRockstar.Texticize.Factories;
using SoftwareRockstar.ComponentModel.Extensibility;
using SoftwareRockstar.ComponentModel.Instrumentation;

namespace SoftwareRockstar.Texticize.SubstitutionProcessors
{
    [Export(typeof(ISubstitutionProcessor))]
    [ExportMetadata(UniquenessEvidenceFields.UniqueName, SystemSubstitutionProcessorNames.Macro)]
    class MacroSubstitutionProcessor : ISubstitutionProcessor
    {
        public ProcessorOutput Process(ProcessorInput input)
        {
            return ProcessMacro(input);
        }

        /// <summary>
        /// Processes macros and performs substitutions
        /// </summary>
        internal ProcessorOutput ProcessMacro(ProcessorInput input, string macroName = null)
        {
            Logger.LogMethodStartup();
            Logger.LogInfo(s => s("Macro: {0}; Target: {0}", macroName ?? "Unknown",  input.Target));

            ProcessorOutput output = new ProcessorOutput();
            output.Result = input.Target;

            try
            {
                macroName = macroName ?? String.Empty;

                Regex regex = new Regex(input.Configuration.MacroRegexPatternFormatted.Insert(1, macroName), input.Configuration.MacroRegexOptions);
                var matches = regex.Matches(output.Result);

                while (matches.Count > 0)
                {
                    for (int i = 0; i < matches.Count; i++)
                    {
                        var match = matches[i];

                        if (match.Success)
                        {
                            string macro = match.Value.Substring(1, match.Value.Length - 2);
                            var processor = MacroProcessorFactory.Create(macro);
                            output.Result = output.Result.Replace(match.Value, processor.GetValue(macro));
                        }
                    }

                    matches = regex.Matches(output.Result);
                }
            }
            catch (ApplicationException ex)
            {
                Logger.LogError(ex);
                output.Exceptions.Add(ex);
            }
            finally
            {
                Logger.LogMethodEnd();
            }

            return output;
        }
    }
}
