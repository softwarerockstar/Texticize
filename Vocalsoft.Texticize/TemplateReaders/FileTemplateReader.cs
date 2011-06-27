//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using Vocalsoft.ComponentModel.Extensibility;

namespace Vocalsoft.Texticize.TemplateReaders
{
    [Export(typeof(ITemplateReader))]
    [ExportMetadata(UniquenessEvidenceFields.UniqueName, SystemTemplateReaders.File)]
    public class FileTemplateReader : ITemplateReader
    {
        string _filePath;
        Encoding _encoding;

        public FileTemplateReader()
        {
            _encoding = Encoding.Default;
        }

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        public Encoding Encoding
        {
            get { return _encoding; }
            set { _encoding = value; }
        }

        public string Read()
        {
            if (String.IsNullOrEmpty(_filePath))
                throw new InvalidOperationException("FilePath cannot be null.");

            return File.ReadAllText(_filePath, _encoding);
        }
    }
}
