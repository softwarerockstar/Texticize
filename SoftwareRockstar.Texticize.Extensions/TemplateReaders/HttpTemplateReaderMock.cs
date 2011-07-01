//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System.ComponentModel.Composition;
using SoftwareRockstar.ComponentModel.Extensibility;
using System;

namespace SoftwareRockstar.Texticize.MockExtensions
{
    [Export(typeof(ITemplateReader))]
    [ExportMetadata(UniquenessEvidenceFields.UniqueName, "SoftwareRockstar.Texticize.MockExtensions.HttpTemplateReaderMock")]
    public class HttpTemplateReaderMock : ITemplateReader
    {
        string _url;

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public string Read()
        {
            return "This template is loaded from " + _url;
        }
    }
}
