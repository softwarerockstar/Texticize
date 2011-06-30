//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
namespace SoftwareRockstar.Texticize
{
    public static class SystemMacroProcessorNames
    {
        public const string DateTime = "DateTime";
        public const string NewLine = "NewLine";
        public const string UserDomainName = "UserDomainName";
        public const string UserName = "UserName";
        public const string Include = "Include";
        public const string Echo = "Echo";
    }

    public static class SystemSubstitutionProcessorNames
    {
        public const string Macro = "Vocalsoft.Texticize.Processors.MacroProcessor";
        public const string Vocabulary = "Vocalsoft.Texticize.Processors.VocabularyProcessor";
    }

    public static class SystemTemplateProcessorNames
    {
        public const string Default = "Default";
    }

    public static class SystemTemplateReaders
    {
        public const string EmbeddedResource = "Vocalsoft.Texticize.TemplateReaders.EmbeddedResourceTemplateReader";
        public const string File = "Vocalsoft.Texticize.TemplateReaders.FileTemplateReader";
        public const string String = "Vocalsoft.Texticize.TemplateReaders.StringTemplateReader";
    }
}
