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
using System.Linq;
using System.Text.RegularExpressions;
using Vocalsoft.ComponentModel;
using Vocalsoft.Serialization;

namespace Vocalsoft.Texticize
{    
    [Serializable]
    /// <summary>
    /// TemplateProcessor
    /// </summary>
    public class TemplateProcessor
    {   
        private ProcessorInput _processInput;
        //private List<Exception> _exceptions;

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
        /// <summary>
        /// For a given type parameter T, creates a mapping between given pattern and a delegate to perform replacement for that pattern.
        /// </summary>
        /// <typeparam name="T">Type parameter.</typeparam>
        /// <param name="pattern">The pattern.</param>
        /// <param name="func">The delegate function that will perform replacement.</param>
        /// <returns>This instance.</returns>
        public TemplateProcessor CreateMap<T>(string pattern, Func<Context<T>, string> func)
        {
            if (String.IsNullOrEmpty(pattern))
                throw new ArgumentNullException("pattern");

            if (_processInput.Maps.ContainsKey(pattern))
                throw new ArgumentException("Specified pattern already exists for given type.");

            // Add key, func to _processInput.Maps dictionary            
            _processInput.Maps.Add(pattern, func);

            return this;
        }

        public TemplateProcessor CreateMap(string pattern, Func<Context<object>, string> func)
        {
            if (String.IsNullOrEmpty(pattern))
                throw new ArgumentNullException("pattern");

            if (_processInput.Maps.ContainsKey(pattern))
                throw new ArgumentException("Specified pattern already exists for given type.");

            // Add key, func to _processInput.Maps dictionary            
            _processInput.Maps.Add(pattern, func);

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
            if (_processInput.Variables.ContainsKey(variableName))
                throw new ArgumentException("Specified variable name already exists.", "variableName");

            _processInput.Variables.Add(variableName, variable);
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
            var plugins = ExtensibilityHelper<IProcessor, IProcessorMetaData>.Current;            
            ProcessorOutput toReturn = new ProcessorOutput();

            ProcessorOutput currentProcessorOutput = new ProcessorOutput { Result = _processInput.Target };
            foreach (var processorName in _processInput.Configuration.ProcessorPipeline)
            {
                var processor = plugins
                    .GetPlugins(s => s.Metadata.Processor == processorName)
                    .FirstOrDefault();
                
                if (processor != null)
                {
                    currentProcessorOutput = processor.Process(_processInput);

                    if (!currentProcessorOutput.IsSuccess)
                    {
                        toReturn.Exceptions.AddRange(currentProcessorOutput.Exceptions);

                        if (!_processInput.Configuration.ContinueOnError)
                            break;
                    }

                    _processInput.Target = currentProcessorOutput.Result;
                }
            }

            toReturn.Result = currentProcessorOutput.Result;
            toReturn.IsSuccess = (toReturn.Exceptions.Count == 0);

            return toReturn;
        }

        #endregion

        public void Save(Uri localPath)
        {
            Save(localPath, TemplateSaveOptions.None);
        }

        public void Save(Uri localPath, TemplateSaveOptions options)
        {
            if (options.HasFlag(TemplateSaveOptions.PreFetchIncludes))
            {
                var output = new Processors.MacroProcessor().ProcessMacro(_processInput, SystemMacros.Include);

                if (output.IsSuccess)
                    _processInput.Target = output.Result;
            }

            BinarySerializer.ObjectToFile(localPath, this);
        }

        public static TemplateProcessor LoadFrom(TemplateProcessor source)
        {
            return BinarySerializer.Base64StringToObject<TemplateProcessor>(BinarySerializer.ObjectToBase64String(source));
        }

        public static TemplateProcessor LoadFrom(Uri localPath)
        {
            return BinarySerializer.FileToObject<TemplateProcessor>(localPath);    
        }

        public static TemplateProcessor LoadFrom(string base64String)
        {
            return BinarySerializer.Base64StringToObject<TemplateProcessor>(base64String);
        }
    }
}
