//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System.Linq;
using SoftwareRockstar.ComponentModel.Extensibility;

namespace SoftwareRockstar.Texticize.Factories
{
    /// <summary>
    /// Factory class for macro processors.
    /// </summary>
    public class MacroProcessorFactory : AbstractExtensionFactory<IMacroProcessor>
    {
        /// <summary>
        /// Creates the specified macro processor using provided unique name.
        /// </summary>
        /// <param name="macroUniqueName">Unique name of the macro.</param>
        /// <returns></returns>
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
