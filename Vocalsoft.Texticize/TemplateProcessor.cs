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
        private RegexOptions _regexOptions;
        private const string DEFAULT_VARIABLE_KEY = "$$__default";

        #region Constructors
        public TemplateProcessor(string template)
        {
            _template = template;
            _maps = new Dictionary<string, Delegate>();
            _variables = new Dictionary<string, object>();
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

        public TemplateProcessor CreateMap(string pattern, Func<Context<object>, string> func)
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
        /// Sets an execution variable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="variable">The variable.</param>
        /// <returns>This instance.</returns>
        public TemplateProcessor SetVariable<T>(string variableName, T variable)
        {
            return SetVariable(variableName, (object)variable);
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
            return SetVariable((object)variable);
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
                    string parameterPattern = @"(?:\[(?<paramsGroup90515721005799>.+?(?:,.+?)*?)\])?";
                    string pattern = originalPattern.Insert(originalPattern.Length-1, parameterPattern);
                    Regex regex = new Regex(pattern, _regexOptions);

                    // Get all regex matches in template
                    var matches = regex.Matches(toReturn);

                    // Process each match...
                    for (int j = 0; j < matches.Count; j++)
                    {
                        var match = matches[j];
                        
                        // Process each group in the match...
                        if (match.Success && match.Groups.Count > 0 && match.Groups.Count > 1)
                        {
                            // Determine variable name
                            string varName = (map.Key.Contains("!")) ?
                                map.Key.Substring(1, map.Key.IndexOf('!') - 1) :
                                _variables.ContainsKey(DEFAULT_VARIABLE_KEY) ?
                                    DEFAULT_VARIABLE_KEY :
                                    "None";

                            // Determine expression found in template to be replaced
                            string expression = (map.Key.Contains("!")) ?
                                expression = String.Format("{0}{1}",
                                    map.Key.Substring(0, 1),
                                    map.Key.Substring(map.Key.IndexOf('!') + 1)) :
                                match.Value;

                            // Determine parameters within expression
                            Dictionary<string, string> parameterDictionary =
                                (match.Groups.Count > 1) ?
                                    match.Groups["paramsGroup90515721005799"].ToParameterDictionary() :
                                    new Dictionary<string, string>();
                            
                            // Determine target variable
                            object target = (_variables.ContainsKey(varName)) ?
                                _variables[varName] :
                                new System.Object();

                            // Create context to be sent to delegate that will provide replacement value
                            IContext context = Utility.CreateContext(
                                variable:target, 
                                variableName: varName, 
                                expression: expression,
                                parameters: parameterDictionary);

                            // Get substitute value
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
