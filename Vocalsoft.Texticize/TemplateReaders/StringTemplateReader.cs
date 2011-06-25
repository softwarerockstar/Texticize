//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System.Text;

namespace Vocalsoft.Texticize.TemplateReaders
{
    public class StringTemplateReader : ITemplateReader
    {
        string _value;

        public StringTemplateReader(StringBuilder value) : this((value != null) ? value.ToString() : "")
        {            
        }

        public StringTemplateReader(string value)
        {
            _value = value;
        }

        public string Read()
        {
            return _value;
        }
    }
}
