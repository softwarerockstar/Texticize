using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vocalsoft.ComponentModel;

namespace Vocalsoft.Texticize.Factories
{
    public static class SystemMacroFactory
    {
        public static ISystemMacro GetMacro(string macroName)
        {
            var plugins = ExtensibilityHelper<ISystemMacro, ISystemMacroMetaData>.Current;

            return plugins
                .GetPlugins(s => macroName.StartsWith(s.Metadata.Macro))
                .FirstOrDefault();
        }
    }
}
