using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vocalsoft.Texticize.TemplateReaders
{
    public class StringTemplateReader : ITemplateReader
    {
        string _value;

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
