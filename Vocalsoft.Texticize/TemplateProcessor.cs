using System;
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

            // Add key to _regexKeys list
            _regexKeys.Add(pattern);

            return this;
        }

        public TemplateProcessor CreateMap(string pattern, Func<Context<object>, string> func)
        {
            if (String.IsNullOrEmpty("pattern"))
                throw new ArgumentNullException("pattern");

            if (_maps.ContainsKey(pattern))
                throw new ArgumentException("Specified pattern already exists for given type.");

            // Add key, func to _maps dictionary            
            _maps.Add(pattern, func);

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

        public TemplateProcessor SetVariable(string variableName, object variable)
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


        public TemplateProcessor SetVariable(object variable)
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
                    string originalPattern = Regex.Escape(map.Key);
                    string parameterPattern = @"(?:\[(.+?(?:,.+?)*?)\])?";
                    string pattern = originalPattern.Insert(originalPattern.Length-1, parameterPattern);
                    Regex regex = new Regex(pattern, _regexOptions);

                    // Get all regex matches in template
                    var matches = regex.Matches(toReturn);
                    Dictionary<string, string> parameterDictionary = new Dictionary<string, string>();

                    // Process each match...
                    for (int j = 0; j < matches.Count; j++)
                    {
                        var match = matches[j];
                        
                        // Process each group in the match...
                        if (match.Success && match.Groups.Count > 0)
                        {
                            if (match.Groups.Count > 1)
                            {
                                var parameters = match.Groups[1].Value.Split(',').Select(s => s.Trim());
                                
                                foreach (string parameter in parameters)
                                {
                                    var paramParts = parameter.Split('=');

                                    if (paramParts.Length > 1)
                                        parameterDictionary.Add(paramParts[0], paramParts[1]);
                                }
                            }
                                

                            string varName;
                            string expression;
                            object target = null;

                            if (map.Key.Contains("!"))
                            {
                                // Variable name
                                varName = map.Key.Substring(1, map.Key.IndexOf('!') - 1);

                                // Expression found in template to be replaced
                                expression = String.Format("{0}{1}",
                                    map.Key.Substring(0, 1),
                                    map.Key.Substring(map.Key.IndexOf('!') + 1));

                                // Find correct variable in the _variables list
                                if (_variables.ContainsKey(varName))
                                    target = _variables[varName];// .Where(s => s.Key == varName).First().Value;
                                else
                                    target = new System.Object();
                            }
                            else
                            {
                                // Find correct variable in the _variables list
                                if (_variables.ContainsKey(DEFAULT_VARIABLE_KEY))
                                {
                                    varName = DEFAULT_VARIABLE_KEY;
                                    target = _variables[DEFAULT_VARIABLE_KEY];
                                }
                                else
                                {
                                    target = new System.Object();
                                    varName = "None";
                                }

                                // Expression found in template to be replaced
                                expression = match.Value;                                
                            }

                            // Create context to be sent to delegate that will provide replacement value
                            IContext context = Utility.CreateContext(
                                variable:target, 
                                variableName: varName, 
                                expression: expression,
                                parameters: parameterDictionary);

                            // Perform substitution using map's delegate function sending in context
                            var substitute = map.Value.DynamicInvoke(context).ToString();

                            // Make replacement in template                            
                            toReturn = toReturn.Replace(match.Value, substitute);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                // Log exception
                Console.Error.WriteLine(ex.ToString());
            }

            return toReturn;
        }
        #endregion
    }
}
