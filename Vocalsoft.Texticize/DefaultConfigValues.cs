//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
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

        public static string[] ProcessorPipelineSteps = new string[] { SystemSubstitutionProcessorNames.Vocabulary, SystemSubstitutionProcessorNames.Macro };

        public static bool ContinueOnError = false;
    }
}
