using System.Globalization;
using System.Reflection;
//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System.Resources;

namespace Vocalsoft.Texticize.TemplateReaders
{
    public class EmbeddedResourceTemplateReader : ITemplateReader
    {
        string _resourceName;
        string _resourceBaseName;
        Assembly _assembly;
        CultureInfo _cultureInfo;

        public EmbeddedResourceTemplateReader(string assemblyName, string resourcePath = null, string resourceName = null, CultureInfo cultureInfo = null)
        {            
            _resourceBaseName = resourcePath;
            _assembly = Assembly.LoadFrom(assemblyName);
            _resourceName = resourceName;
            _cultureInfo = (cultureInfo != null) ? cultureInfo : CultureInfo.CurrentCulture;
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

        public string Read()
        {
            return new ResourceManager(_resourceBaseName, _assembly).GetString(_resourceName, _cultureInfo);
            //return new ResourceManager("Vocalsoft.Texticize.UnitTests.Resources.TestResource", _assembly).GetString(_resourceName, _cultureInfo);
        }
    }
}





