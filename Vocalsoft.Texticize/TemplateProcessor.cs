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
using Vocalsoft.Texticize.Factories;

namespace Vocalsoft.Texticize
{    
    [Serializable]
    /// <summary>
    /// TemplateProcessor
    /// </summary>
    public class TemplateProcessor
    {
        #region Private Members

        private ProcessorInput _processInput; 
        
        #endregion

        #region Constructors
        public TemplateProcessor(ITemplateReader templateReader)
            : this(templateReader, new Configuration())
        {
        }
        
        public TemplateProcessor(ITemplateReader templateReader, Configuration configuration)
        {
            _processInput = new ProcessorInput(configuration) { Target = templateReader.Read() };                
        }
        #endregion

        #region Public Methods

        public TemplateProcessor SetConfiguration(Configuration configuration)
        {
            _processInput.Configuration = configuration;
            return this;
        }

        public TemplateProcessor SetMaps(params KeyValuePair<string, Delegate>[] maps)
        {
            foreach (var item in maps)
            {
                if (String.IsNullOrEmpty(item.Key))
                    throw new ArgumentNullException("Key");

                // Add key, func to _processInput.Maps dictionary
                _processInput.Maps[item.Key] = item.Value;

            }

            return this;
        }

        public TemplateProcessor SetVariables(params KeyValuePair<string, object>[] variables)
        {
            foreach (var item in variables)
                _processInput.Variables[item.Key] = item.Value;

            return this;
        }

        public TemplateProcessor SetVariable(object variable)
        {
            _processInput.Variables[_processInput.Configuration.DefaultVariableKey] = variable;
            return this;
        }

        /// <summary>
        /// Clears all maps.
        /// </summary>
        /// <returns>This instance.</returns>
        public TemplateProcessor ClearMaps()
        {
            _processInput.Maps.Clear();
            return this;
        }

        /// <summary>
        /// Clears all variables.
        /// </summary>
        /// <returns>This instance.</returns>
        public TemplateProcessor ClearVariables()
        {
            _processInput.Variables.Clear();
            return this;
        }

        public ProcessorOutput Process()
        {   
            var exceptions = new List<Exception>();

            ProcessorOutput currentProcessorOutput = new ProcessorOutput { Result = _processInput.Target };
            foreach (var processorName in _processInput.Configuration.ProcessorPipeline)
            {
                var processor = ProcessorFactory.GetProcessor(processorName);                
                if (processor != null)
                {
                    currentProcessorOutput = processor.Process(_processInput);

                    if (!currentProcessorOutput.IsSuccess)
                    {
                        exceptions.AddRange(currentProcessorOutput.Exceptions);

                        if (!_processInput.Configuration.ContinueOnError)
                            break;
                    }

                    _processInput.Target = currentProcessorOutput.Result;
                }
            }

            return new ProcessorOutput(
                currentProcessorOutput.Result, 
                (exceptions.Count == 0), 
                exceptions);
        }

        #endregion

        #region Protected Members

        protected internal ProcessorInput ProcessInput
        {
            get { return _processInput; }
        }

        #endregion

        #region Static Methods

        public static TemplateProcessor FromReader(ITemplateReader reader)
        {
            return new TemplateProcessor(reader);
        }

        #endregion
    }
}
