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
    public class MacroProcessorFactory : AbstractExtensionFactory<IMacroProcessor>
    {
        public static IMacroProcessor Create(string macroUniqueName)
        {
            using (var factory = new MacroProcessorFactory())
            {
                return factory
                    .GetExtensions(s => macroUniqueName.StartsWith(s.Metadata.UniqueName))
                    .FirstOrDefault();
            }
        }
    }
}
