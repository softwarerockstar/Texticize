//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System.Linq;
using Vocalsoft.ComponentModel;

namespace Vocalsoft.Texticize
{
    public static class TemplateProcessorFactory
    {
        public static ITemplateProcessor Create(ITemplateReader reader, string templateProcessorUniqueName = TemplateProcessorNames.Default)
        {
            return GetTemplateProcessor(templateProcessorUniqueName, reader);
        }

        public static ITemplateProcessor GetTemplateProcessor(string processorName, ITemplateReader reader)
        {
            var plugin = ExtensibilityHelper<ITemplateProcessor>.Current.GetPluginByUniqueName(processorName);

            if (reader != null)
                plugin.SetTemplate(reader.Read());
            
            return plugin;

        }

    }
}
