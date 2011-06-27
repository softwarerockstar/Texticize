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
using Vocalsoft.Texticize.SubstitutionProcessors;

namespace Vocalsoft.Texticize
{    
    [Serializable]
    public abstract class AbstractTemplateProcessor : ITemplateProcessor
    {
        #region Private Members
        private ProcessorInput _processInput;
        #endregion

        #region Constructors
        protected AbstractTemplateProcessor()
        {
            _processInput = new ProcessorInput();
        }
        #endregion

        #region Public Methods
        public ITemplateProcessor SetTemplate(string template)
        {
            _processInput.Target = template;
            return this;
        }

        public ITemplateProcessor SetConfiguration(Configuration configuration)
        {
            _processInput.Configuration = configuration;
            return this;
        }

        public ITemplateProcessor SetMaps(params KeyValuePair<string, Delegate>[] maps)
        {
            if (maps != null)
            {
                foreach (var item in maps)
                {
                    if (String.IsNullOrEmpty(item.Key))
                        throw new ArgumentException("One or more keys in maps are null or empty.");

                    // Add key, func to _processInput.Maps dictionary
                    _processInput.Maps[item.Key] = item.Value;
                }
            }

            return this;
        }

        public ITemplateProcessor SetVariables(params KeyValuePair<string, object>[] variables)
        {
            if (variables != null)
            {
                foreach (var item in variables)
                    _processInput.Variables[item.Key] = item.Value;
            }

            return this;
        }

        public ITemplateProcessor SetVariable(object variable)
        {
            _processInput.Variables[_processInput.Configuration.DefaultVariableKey] = variable;
            return this;
        }

        /// <summary>
        /// Clears all maps.
        /// </summary>
        /// <returns>This instance.</returns>
        public ITemplateProcessor ClearMaps()
        {
            _processInput.Maps.Clear();
            return this;
        }

        /// <summary>
        /// Clears all variables.
        /// </summary>
        /// <returns>This instance.</returns>
        public ITemplateProcessor ClearVariables()
        {
            _processInput.Variables.Clear();
            return this;
        }

        public ITemplateProcessor FetchIncludes()
        {
            var output = new MacroSubstitutionProcessor().ProcessMacro(_processInput, MacroProcessorNames.Include);

            if (output.IsSuccess)
                _processInput.Target = output.Result;

            return this;

        }

        public abstract ProcessorOutput Process();

        #endregion

        #region Protected Members
        protected ProcessorInput ProcessInput
        {
            get { return _processInput; }
        }        
        #endregion


    }
}
