﻿//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System.Linq;
using SoftwareRockstar.ComponentModel.Extensibility;

namespace SoftwareRockstar.Texticize
{
    /// <summary>
    /// Factory class for template processors.
    /// </summary>
    public class TemplateProcessorFactory : AbstractExtensionFactory<ITemplateProcessor>
    {
        /// <summary>
        /// Creates the specified template processor using provided unique name.
        /// </summary>
        /// <param name="templateProcessorUniqueName">Name of the template processor unique.</param>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static ITemplateProcessor Create(string templateProcessorUniqueName, ITemplateReader reader = null)
        {
            using (var factory = new TemplateProcessorFactory())
            {
                ITemplateProcessor processor = factory.GetGetExtensionByUniqueName(templateProcessorUniqueName);

                if (reader != null)
                    processor.SetTemplateReader(reader);

                return processor;
            }
        }

        /// <summary>
        /// Creates the default template processor using provided <see cref="ITemplateReader"/>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static ITemplateProcessor CreateDefault(ITemplateReader reader = null)
        {
            return Create(SystemTemplateProcessorNames.Default, reader);
        }

    }
}
