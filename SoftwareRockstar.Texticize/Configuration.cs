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
using System.Text.RegularExpressions;

namespace SoftwareRockstar.Texticize
{
    [Serializable]
    public class Configuration
    {
        char _parameterBeginChar = DefaultConfigValues.ParameterBeginChar;
        char _parameterEndChar = DefaultConfigValues.ParameterEndChar;
        char _parameterSeperatorChar = DefaultConfigValues.ParameterSeperatorChar;
        string _parameterRegexPatternFormat = DefaultConfigValues.ParameterRegexPatternFormat;
        
        string _templateRegexPatternFormat = DefaultConfigValues.TemplateRegexPatternFormat;
        string _templateRegexParamInternalGroupName = DefaultConfigValues.TemplateRegexParamInternalGroupName;

        string _placeHolderBegin = DefaultConfigValues.PlaceHolderBegin;
        string _placeHolderEnd = DefaultConfigValues.PlaceHolderEnd;

        string _macroRegexPattern = DefaultConfigValues.MacroRegexPattern;
        char _macroBeginChar = DefaultConfigValues.MacroBeginChar;
        char _macroEndChar = DefaultConfigValues.MacroEndChar;
        string _macroRegexPatternFormat = DefaultConfigValues.MacroRegexPatternFormat;

        char _propertySeperator = DefaultConfigValues.PropertySeperator;
        string _defaultVariableKey = DefaultConfigValues.DefaultVariableKey;
        string _noVariableName = DefaultConfigValues.NoVariableName;

        RegexOptions _templateRegexOptions = DefaultConfigValues.TemplateRegexOptions;
        RegexOptions _macroRegexOptions = DefaultConfigValues.MacroRegexOptions;

        List<string> _processorPipeline = new List<string>(DefaultConfigValues.ProcessorPipelineSteps);

        bool _continueOnError = DefaultConfigValues.ContinueOnError;

        public string PlaceHolderBegin
        {
            get { return _placeHolderBegin; }
            set { _placeHolderBegin = value; }
        }

        public string PlaceHolderEnd
        {
            get { return _placeHolderEnd; }
            set { _placeHolderEnd = value; }
        }

        public bool ContinueOnError
        {
            get { return _continueOnError; }
            set { _continueOnError = value; }
        }

        public IList<string> ProcessorPipeline
        {
            get { return _processorPipeline; }
        }

        public RegexOptions TemplateRegexOptions
        {
            get { return _templateRegexOptions; }
            set { _templateRegexOptions = value; }
        }

        public RegexOptions MacroRegexOptions
        {
            get { return _macroRegexOptions; }
            set { _macroRegexOptions = value; }
        }

        public string DefaultVariableKey
        {
            get { return _defaultVariableKey; }
            set { _defaultVariableKey = value; }
        }

        public char MacroBeginChar
        {
            get { return _macroBeginChar; }
            set { _macroBeginChar = value; }
        }

        public char MacroEndChar
        {
            get { return _macroEndChar; }
            set { _macroEndChar = value; }
        }

        public string MacroRegexPattern
        {
            get { return _macroRegexPattern; }
            set { _macroRegexPattern = value; }
        }

        protected internal string MacroRegexPatternFormatted
        {
            get 
            {
                return String.Format(
                    _macroRegexPatternFormat,
                    Regex.Escape(_macroBeginChar.ToString()),
                    _macroRegexPattern,
                    Regex.Escape(_macroEndChar.ToString()));
            }            
        }

        public char PropertySeperator
        {
            get { return _propertySeperator; }
            set { _propertySeperator = value; }
        }

        public string ParameterRegexPatternFormat
        {
            get { return _parameterRegexPatternFormat; }
            set { _parameterRegexPatternFormat = value; }
        }

        public char ParameterSeperatorChar
        {
            get { return _parameterSeperatorChar; }
            set { _parameterSeperatorChar = value; }
        }

        protected internal string ParameterRegexPatternFormatted
        {
            get { return String.Format(ParameterRegexPatternFormat, _parameterSeperatorChar); }
        }

        public char ParameterBeginChar
        {
            get { return _parameterBeginChar; }
            set { _parameterBeginChar = value; }
        }

        public char ParameterEndChar
        {
            get { return _parameterEndChar; }
            set { _parameterEndChar = value; }
        }

        protected internal string TemplateRegexPatternFormatted
        {
            get
            {
                return String.Format(
                    _templateRegexPatternFormat,
                    Regex.Escape(_parameterBeginChar.ToString()),
                    _templateRegexParamInternalGroupName,
                    ParameterRegexPatternFormatted,
                    Regex.Escape(_parameterEndChar.ToString()));
            }
        }

        protected internal string TemplateRegexParamInternalGroupName
        {
            get { return _templateRegexParamInternalGroupName; }
            set { _templateRegexParamInternalGroupName = value; }
        }

        protected internal string NoVariableName
        {
            get { return _noVariableName; }
            set { _noVariableName = value; }
        }
    }
}
