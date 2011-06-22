using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Vocalsoft.Texticize.TemplateReaders
{
    public class FileTemplateReader : ITemplateReader
    {
        string _filePath;
        Encoding _encoding;

        public FileTemplateReader(string filePath)
            : this(filePath, Encoding.Default)
        {
        }

        public FileTemplateReader(string filePath, Encoding encoding)
        {
            _filePath = filePath;
            _encoding = encoding;
        }

        public string Read()
        {
            return File.ReadAllText(_filePath, _encoding);
        }
    }
}
