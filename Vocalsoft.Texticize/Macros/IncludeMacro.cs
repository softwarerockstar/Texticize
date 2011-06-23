using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.IO;

namespace Vocalsoft.Texticize.Macros
{
    [Export(typeof(ISystemMacro))]
    [ExportMetadata("Macro", SystemMacros.Include)]
    class IncludeMacro : ISystemMacro
    {
        static Dictionary<string, string> _runtimeFileCache;

        public IncludeMacro()
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
