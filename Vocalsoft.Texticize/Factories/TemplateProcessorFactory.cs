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
    public class TemplateProcessorFactory : AbstractExtensionFactory<ITemplateProcessor>
    {
        public static ITemplateProcessor Create(string templateProcessorUniqueName)
        {
            using (var factory = new TemplateProcessorFactory())
            {
                ITemplateProcessor processor = factory.GetGetExtensionByUniqueName(templateProcessorUniqueName);                
                return processor;
            }
        }

        public static ITemplateProcessor CreateDefault(ITemplateReader reader = null)
        {
            var processor = Create(SystemTemplateProcessorNames.Default);

            if (reader != null)
                processor.SetTemplateReader(reader);

            return processor;
        }

    }
}
