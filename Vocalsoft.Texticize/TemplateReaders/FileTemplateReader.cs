using System.IO;
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
