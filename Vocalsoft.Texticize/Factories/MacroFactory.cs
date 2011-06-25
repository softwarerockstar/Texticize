//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System.Linq;
using Vocalsoft.ComponentModel;

namespace Vocalsoft.Texticize.Factories
{
    public static class MacroFactory
    {
        public static IMacro GetMacro(string macroName)
        {
            var plugins = ExtensibilityHelper<IMacro>.Current;

            return plugins
                .GetPlugins(s => macroName.StartsWith(s.Metadata.UniqueName))
                .FirstOrDefault();
        }
    }
}
