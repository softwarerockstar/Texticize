using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Vocalsoft.Texticize
{
    public class Configurator
    {
        char _templateRegexParamBeginChar = '[';
        char _templateRegexParamEndChar = ']';
        string _templateRegexPattern = ".+?(?:,.+?)*?";
        string _templateRegexPatternFormat = @"(?:{0}(?<{1}>{2}){3})?";
        string _templateRegexParamInternalGroupName = "paramsGroup90515721005799";

        char _propertySeperator = '!';
        string _macroRegexPattern = "%.+?%";
        string _defaultVariableKey = "$$__default";
        string _noVariableName = "None";

        public string DefaultVariableKey
        {
            get { return _defaultVariableKey; }
            set { _defaultVariableKey = value; }
        }
        

        public string MacroRegexPattern
        {
            get { return _macroRegexPattern; }
            set { _macroRegexPattern = value; }
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
