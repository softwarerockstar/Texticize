using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Vocalsoft.Texticize
{
    public class Configuration
    {
        char _templateRegexParamBeginChar = '[';
        char _templateRegexParamEndChar = ']';
        string _templateRegexPattern = ".+?(?:,.+?)*?";
        string _templateRegexPatternFormat = @"(?:{0}(?<{1}>{2}){3})?";
        string _templateRegexParamInternalGroupName = "paramsGroup90515721005799";
        
        string _macroRegexPattern = ".+?";
        char _macroRegexPatternBeginChar = '%';
        char _macroRegexPatternEndChar = '%';
        string _macroRegexPatternFormat = "{0}{1}{2}";

        char _propertySeperator = '!';        
        string _defaultVariableKey = "$$__default";
        string _noVariableName = "None";

        RegexOptions _templateRegexOptions = RegexOptions.None;
        RegexOptions _macroRegexOptions = RegexOptions.None;

        List<string> _processorPipeline = new List<string> { "Vocabulary", "Macro" };

        public List<string> ProcessorPipeline
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
