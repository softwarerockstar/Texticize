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
    public class TemplateReaderFactory : AbstractExtensionFactory<ITemplateReader>
    {
        public static ITemplateReader Create(string readerName)
        {
            using (var factory = new TemplateReaderFactory())
            {
                return factory.GetGetExtensionByUniqueName(readerName);
            }

        }

        public static ITemplateReader CreateStringTemplateReader(string input)
        {
            var reader = Create(SystemTemplateReaders.String) as StringTemplateReader;
            reader.Value = input;
            return reader;
        }


        public static ITemplateReader CreateStringTemplateReader(StringBuilder input)
        {
            return (input != null) ? CreateStringTemplateReader(input.ToString()) : null;
        }


        public static ITemplateReader CreateFileTemplateReader(string filePath, Encoding encoding = null)
        {
            var reader = Create(SystemTemplateReaders.File) as FileTemplateReader;
            reader.FilePath = filePath;

            if (encoding != null)
                reader.Encoding = encoding;
            
            return reader;
        }

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
