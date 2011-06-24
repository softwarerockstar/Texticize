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
using Vocalsoft.Texticize.Factories;

namespace Vocalsoft.Texticize.Processors
{
    [Export(typeof(IProcessor))]
    [ExportMetadata("UniqueName", ProcessorNames.Macro)]
    class MacroProcessor : IProcessor
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
                            var processor = MacroFactory.GetMacro(macro);
                            output.Result = output.Result.Replace(match.Value, processor.GetValue(macro));
                        }
                    }

                    matches = regex.Matches(output.Result);
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
