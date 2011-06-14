﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Vocalsoft.Texticize
{
    /// <summary>
    /// TemplateProcessor
    /// </summary>
    public class TemplateProcessor
    {   
        private string _template;
        private Dictionary<string, Delegate> _maps;
        private Dictionary<string, object> _variables;
        private List<string> _regexKeys;
        private RegexOptions _regexOptions;
        private const string DEFAULT_VARIABLE_KEY = "$$__default";

        #region Constructors        
        public TemplateProcessor(string template)
        {
            _template = template;
            _maps = new Dictionary<string, Delegate>();
            _variables = new Dictionary<string, object>();
            _regexKeys = new List<string>();
            _regexOptions = RegexOptions.None;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// For a given type parameter T, creates a mapping between given pattern and a delegate to perform replacement for that pattern.
        /// </summary>
        /// <typeparam name="T">Type parameter.</typeparam>
        /// <param name="pattern">The pattern.</param>
        /// <param name="func">The delegate function that will perform replacement.</param>
        /// <returns>This instance.</returns>
        public TemplateProcessor CreateMap<T>(string pattern, Func<Context<T>, string> func)
        {
            if (String.IsNullOrEmpty("pattern"))
                throw new ArgumentNullException("pattern");

            if (_maps.ContainsKey(pattern))
                throw new ArgumentException("Specified pattern already exists for given type.");

            // Add key, func to _maps dictionary            
            _maps.Add(pattern, func);

            return this;
        }

        /// <summary>
        /// For a given type parameter T, creates a mapping between given pattern and a delegate to perform replacement for that pattern.
        /// </summary>
        /// <typeparam name="T">Type parameter.</typeparam>
        /// <param name="pattern">The pattern.</param>
        /// <param name="func">The delegate function that will perform replacement.</param>
        /// <returns>This instance.</returns>
        public TemplateProcessor CreatePatternMap<T>(string pattern, Func<Context<T>, string> func)
        {
            // Create map
            CreateMap<T>(pattern, func);

            // Add key to _regexKeys list
            _regexKeys.Add(pattern);

            return this;
        }

        /// <summary>
        /// Sets an execution variable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="variable">The variable.</param>
        /// <returns>This instance.</returns>
        public TemplateProcessor SetVariable<T>(string variableName, T variable)
        {
            if (_variables.ContainsKey(variableName))
                throw new ArgumentException("Specified variable name already exists.", "variableName");

            _variables.Add(variableName, variable);
            return this;
        }

        /// <summary>
        /// Sets default execution variable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="variable">The variable.</param>
        /// <returns>This instance.</returns>
        public TemplateProcessor SetVariable<T>(T variable)
        {
            _variables[DEFAULT_VARIABLE_KEY] = variable;
            return this;
        }


        /// <summary>
        /// Sets the Regex options used in matching expressions in the template.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public TemplateProcessor SetRegExOptions(RegexOptions options)
        {
            _regexOptions = options;
            return this;
        }

        /// <summary>
        /// Clears all maps.
        /// </summary>
        /// <returns>This instance.</returns>
        public TemplateProcessor ClearMaps()
        {
            _maps.Clear();
            _regexKeys.Clear();
            return this;
        }

        /// <summary>
        /// Clears all variables.
        /// </summary>
        /// <returns>This instance.</returns>
        public TemplateProcessor ClearVariables()
        {
            _variables.Clear();
            return this;
        }

        /// <summary>
        /// Processes the template and performs substitution
        /// </summary>
        /// <returns>String result.</returns>
        public string ProcessTemplate()
        {
            string toReturn = _template;            

            try
            {
                foreach (var map in _maps)
                {
                    // If map was created as a pattern map, then map's key should exist in regexKeys list.
                    // In that case create a regular expression regex, otherwise create regex excaping
                    // any special characters implying non-regular expression text                    
                    Regex regex = _regexKeys.Contains(map.Key) ? new Regex(map.Key, _regexOptions) : new Regex(Regex.Escape(map.Key), _regexOptions);

                    // Get all regex matches in template
                    var matches = regex.Matches(toReturn);

                    // Process each match...
                    for (int j = 0; j < matches.Count; j++)
                    {
                        var match = matches[j];

                        List<string> groups = new List<string>();
                        
                        // Process each group in the match...
                        if (match.Success && match.Groups.Count > 0)
                        {
                            for (int i = 1; i < match.Groups.Count; i++)
                                groups.Add(match.Groups[i].Value);

                            string varName;
                            string expression;
                            object target;

                            if (map.Key.Contains("!"))
                            {
                                // Variable name
                                varName = map.Key.Substring(1, map.Key.IndexOf('!') - 1);

                                // Expression found in template to be replaced
                                expression = String.Format("{0}{1}",
                                    map.Key.Substring(0, 1),
                                    map.Key.Substring(map.Key.IndexOf('!') + 1));

                                // Find correct variable in the _variables list
                                target = _variables.Where(s => s.Key == varName).First().Value;
                            }
                            else
                            {
                                // Variable name
                                varName = map.Key;

                                // Expression found in template to be replaced
                                expression = match.Value;
                                
                                // Find correct variable in the _variables list
                                target = _variables.Where(s => s.Key == DEFAULT_VARIABLE_KEY).First().Value;
                            }

                            // Create context to be sent to delegate that will provide replacement value
                            IContext context = Utility.CreateContext(
                                variable:target, 
                                variableName: varName, 
                                expression: expression, 
                                regexGroups: groups);

                            // Perform substitution using map's delegate function sending in context
                            var substitute = map.Value.DynamicInvoke(context).ToString();

                            // Make replacement in template                            
                            toReturn = toReturn.Replace(match.Value, substitute);
                        }
                    }

                }
            }
            catch
            {
                // Log exception
            }

            return toReturn;
        }
        #endregion
    }
}
