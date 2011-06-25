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

namespace Vocalsoft.Texticize
{
    [Serializable]
    public class Configuration
    {
        char _templateRegexParamBeginChar = DefaultConfigValues.TemplateRegexParamBeginChar;
        char _templateRegexParamEndChar = DefaultConfigValues.TemplateRegexParamEndChar;
        string _templateRegexPattern = DefaultConfigValues.TemplateRegexPattern;
        string _templateRegexPatternFormat = DefaultConfigValues.TemplateRegexPatternFormat;
        string _templateRegexParamInternalGroupName = DefaultConfigValues.TemplateRegexParamInternalGroupName;

        string _macroRegexPattern = DefaultConfigValues.MacroRegexPattern;
        char _macroRegexPatternBeginChar = DefaultConfigValues.MacroRegexPatternBeginChar;
        char _macroRegexPatternEndChar = DefaultConfigValues.MacroRegexPatternEndChar;
        string _macroRegexPatternFormat = DefaultConfigValues.MacroRegexPatternFormat;

        char _propertySeperator = DefaultConfigValues.PropertySeperator;
        string _defaultVariableKey = DefaultConfigValues.DefaultVariableKey;
        string _noVariableName = DefaultConfigValues.NoVariableName;

        RegexOptions _templateRegexOptions = DefaultConfigValues.TemplateRegexOptions;
        RegexOptions _macroRegexOptions = DefaultConfigValues.MacroRegexOptions;

        List<string> _processorPipeline = new List<string>(DefaultConfigValues.ProcessorPipelineSteps);

        bool _continueOnError = DefaultConfigValues.ContinueOnError;

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

        public char MacroRegexPatternBeginChar
        {
            get { return _macroRegexPatternBeginChar; }
            set { _macroRegexPatternBeginChar = value; }
        }

        public char MacroRegexPatternEndChar
        {
            get { return _macroRegexPatternEndChar; }
            set { _macroRegexPatternEndChar = value; }
        }

        public string MacroRegexPattern
        {
            get { return _macroRegexPattern; }
            set { _macroRegexPattern = value; }
        }

        internal string MacroRegexPatternFormatted
        {
            get 
            {
                return String.Format(
                    _macroRegexPatternFormat,
                    Regex.Escape(_macroRegexPatternBeginChar.ToString()),
                    _macroRegexPattern,
                    Regex.Escape(_macroRegexPatternEndChar.ToString()));
            }            
        }

        public char PropertySeperator
        {
            get { return _propertySeperator; }
            set { _propertySeperator = value; }
        }

        public string TemplateRegexPattern
        {
            get { return _templateRegexPattern; }
            set { _templateRegexPattern = value; }
        }

        public char TemplateRegexParamBeginChar
        {
            get { return _templateRegexParamBeginChar; }
            set { _templateRegexParamBeginChar = value; }
        }

        public char TemplateRegexParamEndChar
        {
            get { return _templateRegexParamEndChar; }
            set { _templateRegexParamEndChar = value; }
        }

        internal string TemplateRegexPatternFormatted
        {
            get
            {
                return String.Format(
                    _templateRegexPatternFormat,
                    Regex.Escape(_templateRegexParamBeginChar.ToString()),
                    _templateRegexParamInternalGroupName,
                    _templateRegexPattern,
                    Regex.Escape(_templateRegexParamEndChar.ToString()));
            }
        }

        internal string TemplateRegexParamInternalGroupName
        {
            get { return _templateRegexParamInternalGroupName; }
            set { _templateRegexParamInternalGroupName = value; }
        }

        internal string NoVariableName
        {
            get { return _noVariableName; }
            set { _noVariableName = value; }
        }
    }
}
