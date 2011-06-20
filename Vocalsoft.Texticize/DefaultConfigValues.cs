using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Vocalsoft.Texticize
{
    static class DefaultConfigValues
    {
        public static char TemplateRegexParamBeginChar = '[';
        public static char TemplateRegexParamEndChar = ']';
        public static string TemplateRegexPattern = ".+?(?:,.+?)*?";
        public static string TemplateRegexPatternFormat = @"(?:{0}(?<{1}>{2}){3})?";
        public static string TemplateRegexParamInternalGroupName = "paramsGroup90515721005799";

        public static string MacroRegexPattern = ".+?";
        public static char MacroRegexPatternBeginChar = '%';
        public static char MacroRegexPatternEndChar = '%';
        public static string MacroRegexPatternFormat = "{0}{1}{2}";

        public static char PropertySeperator = '!';
        public static string DefaultVariableKey = "$$__default";
        public static string NoVariableName = "None";

        public static RegexOptions TemplateRegexOptions = RegexOptions.None;
        public static RegexOptions MacroRegexOptions = RegexOptions.None;

        public static string[] ProcessorPipelineSteps = new string[] { "Vocabulary", "Macro" };

        public static bool ContinueOnError = false;
    }
}
