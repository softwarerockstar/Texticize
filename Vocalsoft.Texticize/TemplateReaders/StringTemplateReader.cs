//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System.ComponentModel.Composition;
using System.Text;
using Vocalsoft.ComponentModel;

namespace Vocalsoft.Texticize.TemplateReaders
{
    [Export(typeof(ITemplateReader))]
    [ExportMetadata(UniquenessEvidenceFields.UniqueName, SystemTemplateReaders.String)]
    public class StringTemplateReader : ITemplateReader
    {
        string _value;

        public StringTemplateReader()
        {
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public string Read()
        {
            return _value;
        }
    }
}
