﻿//-----------------------------------------------------------------------
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
using System.Linq;
using System.Text.RegularExpressions;
using SoftwareRockstar.ComponentModel.Extensibility;
using System.Collections;

namespace SoftwareRockstar.Texticize.SubstitutionProcessors
{
    [Export(typeof(ISubstitutionProcessor))]
    [ExportMetadata(UniquenessEvidenceFields.UniqueName, SystemSubstitutionProcessorNames.Vocabulary)]
    class VocabularySubstitutionProcessor : ISubstitutionProcessor
    {
        public ProcessorOutput Process(ProcessorInput input)
        {
            Logger.LogMethodStartup();
            ProcessorOutput output = new ProcessorOutput();

            if (input != null)
            {
                try
                {
                    // Process regular vocabulary with provided maps
                    Logger.LogInfo("Processing Maps");                    
                    output.Combine(ProcessMaps(input));

                    // If no mpas specified but a default variable is specified then see if default variable is a 
                    // dictionary and then perform dictionary subsitutions
                    if (input.Maps.Count == 0 && input.Variables.ContainsKey(input.Configuration.DefaultVariableKey))
                    {
                        if (output.IsSuccess || input.Configuration.ContinueOnError)
                        {
                            input.Target = output.Result;
                            Logger.LogInfo("Processing Default Variables");
                            output.Combine(ProcessDefaultVariableDictionary(input));
                        }
                    }
                }
                catch (ApplicationException ex)
                {
                    Logger.LogError(ex);
                    output.Exceptions.Add(ex);
                }
            }

            Logger.LogMethodEnd();
            return output;
        }

        /// <summary>
        /// If default variable is an IDictionary performs subsitutions based on it's keys and values.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        protected ProcessorOutput ProcessDefaultVariableDictionary(ProcessorInput input)
        {
            ProcessorOutput output = new ProcessorOutput();

            try
            {
                output.Result = input.Target;
                var dictionary = input.Variables[input.Configuration.DefaultVariableKey] as IDictionary;
                
                if (dictionary != null)
                    foreach (DictionaryEntry item in dictionary)
                        output.Result = output.Result.Replace(
                            input.Configuration.PlaceHolderBegin + item.Key.ToString() + input.Configuration.PlaceHolderEnd, 
                            item.Value.ToString());
            }
            catch (ApplicationException ex)
            {
                output.Exceptions.Add(ex);
            }
            
            return output;
        }
        /// <summary>
        /// Iterates through provided maps and processes the template performing substitutions specified by maps.
        /// </summary>
        /// <returns>String result.</returns>
        protected ProcessorOutput ProcessMaps(ProcessorInput input)
        {
            ProcessorOutput output = new ProcessorOutput();
            output.Result = input.Target;

            try
            {
                foreach (var map in input.Maps)
                {
                    // Escape begin and end tokens
                    string placeHolderBegin = Regex.Escape(input.Configuration.PlaceHolderBegin);
                    string placeHolderEnd = Regex.Escape(input.Configuration.PlaceHolderEnd);

                    // Generate pattern based on mapy's key and begin and end tokens
                    string originalPattern = placeHolderBegin + Regex.Escape(map.Key) + placeHolderEnd;

                    // Insert parameters pattern into generated pattern 
                    string parameterPattern = input.Configuration.TemplateRegexPatternFormatted;
                    string pattern = originalPattern.Insert(originalPattern.Length - placeHolderEnd.Length, parameterPattern);
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
                                map.Key.Substring(0, map.Key.IndexOf(input.Configuration.PropertySeperator)) :
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
                                    .ToParameterDictionary(input.Configuration.ParameterSeperatorChar) :
                                    new Dictionary<string, string>();

                            // Determine target variable
                            object target = (input.Variables.ContainsKey(varName)) ?
                                input.Variables[varName] :
                                new System.Object();

                            // Create context to be sent to delegate that will provide replacement value
                            IContext context = CachedContext.GetContext(
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
            catch (ApplicationException ex)
            {
                output.Exceptions.Add(ex);
            }

            return output;
        }
    }
}
