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
using System.Globalization;
using System.Linq;
using System.Text;
using SoftwareRockstar.ComponentModel.Extensibility;
using SoftwareRockstar.Texticize.TemplateReaders;

namespace SoftwareRockstar.Texticize
{
    /// <summary>
    /// Factory class for template readers.
    /// </summary>
    public class TemplateReaderFactory : AbstractExtensionFactory<ITemplateReader>
    {
        /// <summary>
        /// Creates the specified template reader using provided name.
        /// </summary>
        /// <param name="readerName">Name of the template reader.</param>
        /// <returns></returns>
        public static ITemplateReader Create(string readerName)
        {
            using (var factory = new TemplateReaderFactory())
            {
                return factory.GetGetExtensionByUniqueName(readerName);
            }

        }

        /// <summary>
        /// Creates instance of <see cref="StringTemplateReader"/> using provided <see cref="System.String"/>.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns></returns>
        public static ITemplateReader CreateStringTemplateReader(string input)
        {
            var reader = Create(SystemTemplateReaders.String) as StringTemplateReader;
            reader.Value = input;
            return reader;
        }


        /// <summary>
        /// Creates instance of <see cref="StringTemplateReader"/> using provided <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="input">The input <see cref="StringBuilder"/>.</param>
        /// <returns></returns>
        public static ITemplateReader CreateStringTemplateReader(StringBuilder input)
        {
            return (input != null) ? CreateStringTemplateReader(input.ToString()) : null;
        }

        /// <summary>
        /// Creates instance of <see cref="FileTemplateReader"/> using provided file path and <see cref="System.Text.Encoding"/>.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static ITemplateReader CreateFileTemplateReader(string filePath, Encoding encoding = null)
        {
            var reader = Create(SystemTemplateReaders.File) as FileTemplateReader;
            reader.FilePath = filePath;

            if (encoding != null)
                reader.Encoding = encoding;
            
            return reader;
        }

        /// <summary>
        /// Creates instance of <see cref="EmbeddedResourceTemplateReader"/>.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="resourceBaseName">Name of the resource base.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="cultureInfo">The culture info.</param>
        /// <returns></returns>
        public static ITemplateReader CreateEmbeddedResourceTemplateReader(
            string assemblyName, 
            string resourceBaseName = null, 
            string resourceName = null, 
            CultureInfo cultureInfo = null)
        {
            var reader = Create(SystemTemplateReaders.EmbeddedResource) as EmbeddedResourceTemplateReader;
            reader.AssemblyName = assemblyName;
            reader.ResourceBaseName = resourceBaseName;
            reader.ResourceName = resourceName;
            reader.CultureInfo = cultureInfo;

            return reader;
        }

    }
}
