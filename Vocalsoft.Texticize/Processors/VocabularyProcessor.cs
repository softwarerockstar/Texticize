﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel.Composition;

namespace Vocalsoft.Texticize.Processors
{
    [Export(typeof(IProcessor))]
    [ExportMetadata("Processor", "Vocabulary")]
    class VocabularyProcessor : IProcessor
    {
        /// <summary>
        /// Processes the template and performs substitution
        /// </summary>
        /// <returns>String result.</returns>
        public ProcessorOutput Process(ProcessorInput input)
        {
            ProcessorOutput output = new ProcessorOutput();
            output.Result = input.Target;

            try
            {
                foreach (var map in input.Maps)
                {
                    string originalPattern = Regex.Escape(map.Key);
                    string parameterPattern = input.Configuration.TemplateRegexPatternFormatted;
                    string pattern = originalPattern.Insert(originalPattern.Length - 1, parameterPattern);
                    Regex regex = new Regex(pattern, input.Configuration.TemplateRegexOptions);

                    // Get all regex matches in template
                    var matches = regex.Matches(output.Result);

                    // Process each match...
                    for (int j = 0; j < matches.Count; j++)
                    {
                        var match = matches[j];

                        // Process each group in the match...
                        if (match.Success && match.Groups.Count > 0 && match.Groups.Count > 1)
                        {
                            // Determine variable name
                            string varName = (map.Key.Contains(input.Configuration.PropertySeperator)) ?
                                map.Key.Substring(1, map.Key.IndexOf(input.Configuration.PropertySeperator) - 1) :
                                input.Variables.ContainsKey(input.Configuration.DefaultVariableKey) ?
                                    input.Configuration.DefaultVariableKey :
                                    input.Configuration.NoVariableName;

                            // Determine expression found in template to be replaced
                            string expression = (map.Key.Contains(input.Configuration.PropertySeperator)) ?
                                expression = String.Format("{0}{1}",
                                    map.Key.Substring(0, 1),
                                    map.Key.Substring(map.Key.IndexOf(input.Configuration.PropertySeperator) + 1)) :
                                match.Value;

                            // Determine parameters within expression
                            Dictionary<string, string> parameterDictionary =
                                (match.Groups.Count > 1) ?
                                    match.Groups[input.Configuration.TemplateRegexParamInternalGroupName]
                                    .ToParameterDictionary() :
                                    new Dictionary<string, string>();

                            // Determine target variable
                            object target = (input.Variables.ContainsKey(varName)) ?
                                input.Variables[varName] :
                                new System.Object();

                            // Create context to be sent to delegate that will provide replacement value
                            IContext context = Utility.CreateContext(
                                variable: target,
                                variableName: varName,
                                expression: expression,
                                parameters: parameterDictionary);

                            // Get substitute value
                            var substitute = map.Value.DynamicInvoke(context).ToString();

                            // Make replacement in template                            
                            output.Result = output.Result.Replace(match.Value, substitute);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                // Log exception
                Console.Error.WriteLine(ex.ToString());
            }

            return output;
        }
    }
}