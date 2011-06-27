//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.ComponentModel.Composition;
using Vocalsoft.ComponentModel.Extensibility;

namespace Vocalsoft.Texticize.TemplateReaders
{
    [Export(typeof(ITemplateReader))]
    [ExportMetadata(UniquenessEvidenceFields.UniqueName, SystemTemplateReaders.EmbeddedResource)]
    public class EmbeddedResourceTemplateReader : ITemplateReader
    {
        string _resourceName;
        string _resourceBaseName;
        string _assemblyName;
        CultureInfo _cultureInfo;

        public EmbeddedResourceTemplateReader()
        {            
        }

        public string ResourceBaseName
        {
            get { return _resourceBaseName; }
            set { _resourceBaseName = value; }
        }

        public string ResourceName
        {
            get { return _resourceName; }
            set { _resourceName = value; }
        }

        public string AssemblyName
        {
            get { return _assemblyName; }
            set { _assemblyName = value; }
        }

        public CultureInfo CultureInfo
        {
            get { return _cultureInfo; }
            set { _cultureInfo = value; }
        }

        public string Read()
        {
            if (String.IsNullOrEmpty(_assemblyName))
                throw new InvalidOperationException("AssemblyName cannot be null");

            if (String.IsNullOrEmpty(_resourceBaseName))
                throw new InvalidOperationException("ResourceBaseName cannot be null");

            if (String.IsNullOrEmpty(_resourceName))
                throw new InvalidOperationException("ResourceName cannot be null");

            if (_cultureInfo == null)
                _cultureInfo = CultureInfo.CurrentCulture;

            return new ResourceManager(
                _resourceBaseName, 
                Assembly.LoadFrom(_assemblyName)).GetString(_resourceName, _cultureInfo);
        }
    }
}





