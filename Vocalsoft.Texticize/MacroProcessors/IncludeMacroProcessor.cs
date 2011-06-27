//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using Vocalsoft.ComponentModel;

namespace Vocalsoft.Texticize.MacroProcessors
{
    [Export(typeof(IMacroProcessor))]
    [ExportMetadata(UniquenessEvidenceFields.UniqueName, MacroProcessorNames.Include)]
    class IncludeMacroProcessor : IMacroProcessor
    {
        static Dictionary<string, string> _runtimeFileCache;

        public IncludeMacroProcessor()
        {
            _runtimeFileCache = new Dictionary<string, string>();
        }

        public string GetValue(string macro)
        {            
            string filePath = MacroHelper.ParseParameters(macro, ' ')[0];

            if (!_runtimeFileCache.ContainsKey(filePath))
            {
                string content = File.ReadAllText(filePath);
                _runtimeFileCache[filePath] = content;
            }

            return _runtimeFileCache[filePath];
        }
    }
}
